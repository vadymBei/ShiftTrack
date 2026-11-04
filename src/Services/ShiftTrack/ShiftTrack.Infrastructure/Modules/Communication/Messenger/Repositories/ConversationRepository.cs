using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;
using ShiftTrack.Infrastructure.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Modules.Communication.Messenger.Repositories;

public class ConversationRepository(
    IApplicationDbContext applicationDbContext) : IConversationRepository
{
    public async Task<IReadOnlyList<ConversationSummary>> GetSummaries(long employeeId, int take, long? conversationId,
        CancellationToken cancellationToken)
    {
        var memberships = applicationDbContext.ConversationMembers
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId);

        memberships = conversationId is not null
            ? memberships.Where(x => x.ConversationId == conversationId)
            // An empty direct chat is shown only to the one who opened it.
            : memberships.Where(x => x.Conversation.Type == ConversationType.Group
                                     || x.Conversation.LastMessageId != null
                                     || x.Conversation.AuthorId == employeeId);

        var rows = await memberships
            .OrderByDescending(x => x.Conversation.LastMessageAt ?? x.Conversation.CreatedAt)
            .ThenByDescending(x => x.ConversationId)
            .Take(take)
            .Select(x => new
            {
                x.ConversationId,
                UnreadCount = applicationDbContext.Messages.Count(m =>
                    m.ConversationId == x.ConversationId
                    && m.AuthorId != employeeId
                    && m.DeletedAt == null
                    && (x.LastReadMessageId == null || m.Id > x.LastReadMessageId)),
                HasUnreadMention = applicationDbContext.MessageMentions.Any(m =>
                    m.EmployeeId == employeeId
                    && m.Message.ConversationId == x.ConversationId
                    && m.Message.DeletedAt == null
                    && (x.LastReadMessageId == null || m.MessageId > x.LastReadMessageId))
            })
            .ToListAsync(cancellationToken);

        if (rows.Count == 0)
            return [];

        var ids = rows.Select(x => x.ConversationId).ToList();

        var conversations = await applicationDbContext.Conversations
            .AsNoTracking()
            .Include(x => x.Members)
            .ThenInclude(x => x.Employee)
            .ThenInclude(x => x.Position)
            .Include(x => x.Members)
            .ThenInclude(x => x.Employee)
            .ThenInclude(x => x.Department)
            .ThenInclude(x => x.Unit)
            .Where(x => ids.Contains(x.Id))
            .AsSplitQuery()
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        var lastMessageIds = conversations.Values
            .Where(x => x.LastMessageId != null)
            .Select(x => x.LastMessageId.Value)
            .ToList();

        var lastMessages = await applicationDbContext.Messages
            .AsNoTracking()
            .Include(x => x.Author)
            .Include(x => x.Attachments)
            .Include(x => x.Mentions)
            .ThenInclude(x => x.Employee)
            .Where(x => lastMessageIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        return rows
            .Where(x => conversations.ContainsKey(x.ConversationId))
            .Select(x =>
            {
                var conversation = conversations[x.ConversationId];

                return new ConversationSummary
                {
                    Conversation = conversation,
                    LastMessage = conversation.LastMessageId is { } lastId ? lastMessages.GetValueOrDefault(lastId) : null,
                    UnreadCount = x.UnreadCount,
                    HasUnreadMention = x.HasUnreadMention
                };
            })
            .ToList();
    }

    public Task<Conversation> GetForUpdate(long id, CancellationToken cancellationToken)
    {
        return applicationDbContext.Conversations
            .Include(x => x.Members)
            .ThenInclude(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<bool> IsMember(long conversationId, long employeeId, CancellationToken cancellationToken)
    {
        return applicationDbContext.ConversationMembers
            .AnyAsync(x => x.ConversationId == conversationId && x.EmployeeId == employeeId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<long>> GetMemberIds(long conversationId, CancellationToken cancellationToken)
    {
        return await applicationDbContext.ConversationMembers
            .Where(x => x.ConversationId == conversationId)
            .Select(x => x.EmployeeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<long> GetOrCreateDirect(long employeeId, long otherEmployeeId, CancellationToken cancellationToken)
    {
        var key = Conversation.DirectKeyFor(employeeId, otherEmployeeId);

        var id = await FindDirect(key, cancellationToken);

        if (id is not null)
            return id.Value;

        var now = DateTime.UtcNow;
        var conversation = new Conversation
        {
            Type = ConversationType.Direct,
            DirectKey = key,
            Members =
            [
                new ConversationMember { EmployeeId = employeeId, Role = ConversationMemberRole.Member, JoinedAt = now },
                new ConversationMember { EmployeeId = otherEmployeeId, Role = ConversationMemberRole.Member, JoinedAt = now }
            ]
        };

        applicationDbContext.Conversations.Add(conversation);

        try
        {
            await applicationDbContext.SaveChangesAsync(cancellationToken);

            return conversation.Id;
        }
        catch (DbUpdateException)
        {
            // Both opened the chat at the same moment: the unique DirectKey let one of them in.
            // Inside a transaction EF rolls back to its savepoint, so the transaction can go on.
            foreach (var member in conversation.Members.ToList())
                applicationDbContext.ConversationMembers.Entry(member).State = EntityState.Detached;

            applicationDbContext.Conversations.Entry(conversation).State = EntityState.Detached;

            return await FindDirect(key, cancellationToken) ?? throw new InvalidOperationException(
                $"Direct conversation {key} is neither created nor found.");
        }
    }

    public async Task Add(Conversation conversation, CancellationToken cancellationToken)
    {
        applicationDbContext.Conversations.Add(conversation);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Remove(Conversation conversation, CancellationToken cancellationToken)
    {
        applicationDbContext.Conversations.Remove(conversation);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task Save(CancellationToken cancellationToken)
    {
        return applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task SetLastMessage(long conversationId, long messageId, DateTime sentAt, CancellationToken cancellationToken)
    {
        return applicationDbContext.Conversations
            .Where(x => x.Id == conversationId && (x.LastMessageId == null || x.LastMessageId < messageId))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.LastMessageId, messageId)
                .SetProperty(x => x.LastMessageAt, sentAt), cancellationToken);
    }

    public async Task<bool> MarkRead(long conversationId, long employeeId, long messageId,
        CancellationToken cancellationToken)
    {
        var updated = await applicationDbContext.ConversationMembers
            .IgnoreQueryFilters()
            .Where(x => x.ConversationId == conversationId
                        && x.EmployeeId == employeeId
                        && (x.LastReadMessageId == null || x.LastReadMessageId < messageId))
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.LastReadMessageId, messageId), cancellationToken);

        return updated > 0;
    }

    public async Task<MessengerUnreadCount> CountUnread(long employeeId, CancellationToken cancellationToken)
    {
        var counts = await applicationDbContext.ConversationMembers
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .Select(x => applicationDbContext.Messages.Count(m =>
                m.ConversationId == x.ConversationId
                && m.AuthorId != employeeId
                && m.DeletedAt == null
                && (x.LastReadMessageId == null || m.Id > x.LastReadMessageId)))
            .Where(x => x > 0)
            .ToListAsync(cancellationToken);

        return new MessengerUnreadCount(counts.Count, counts.Sum());
    }

    public async Task<bool> SetMuted(long conversationId, long employeeId, bool muted,
        CancellationToken cancellationToken)
    {
        var updated = await applicationDbContext.ConversationMembers
            .IgnoreQueryFilters()
            .Where(x => x.ConversationId == conversationId && x.EmployeeId == employeeId && x.IsMuted != muted)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.IsMuted, muted), cancellationToken);

        return updated > 0;
    }

    private Task<long?> FindDirect(string key, CancellationToken cancellationToken)
    {
        return applicationDbContext.Conversations
            .AsNoTracking()
            .Where(x => x.DirectKey == key)
            .Select(x => (long?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
