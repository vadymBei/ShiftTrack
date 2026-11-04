using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Modules.Communication.Messenger.Repositories;

public class MessageRepository(
    IApplicationDbContext applicationDbContext) : IMessageRepository
{
    public async Task<IReadOnlyList<Message>> GetPage(long conversationId, long? beforeId, int take,
        CancellationToken cancellationToken)
    {
        var query = WithDetails(applicationDbContext.Messages.AsNoTracking())
            .Where(x => x.ConversationId == conversationId);

        if (beforeId is not null)
            query = query.Where(x => x.Id < beforeId);

        var page = await query
            .OrderByDescending(x => x.Id)
            .Take(take)
            .ToListAsync(cancellationToken);

        page.Reverse();

        return page;
    }

    public async Task<IReadOnlyList<Message>> GetPageUntil(long conversationId, long? beforeId, long untilId, int take,
        int maxRange, CancellationToken cancellationToken)
    {
        var query = WithDetails(applicationDbContext.Messages.AsNoTracking())
            .Where(x => x.ConversationId == conversationId);

        if (beforeId is not null)
            query = query.Where(x => x.Id < beforeId);

        var range = await query
            .Where(x => x.Id >= untilId)
            .OrderByDescending(x => x.Id)
            .Take(maxRange)
            .ToListAsync(cancellationToken);

        // Past the limit the range doesn't reach the message: the client asks again from the oldest one.
        var older = range.Count < maxRange
            ? await query
                .Where(x => x.Id < untilId)
                .OrderByDescending(x => x.Id)
                .Take(take)
                .ToListAsync(cancellationToken)
            : [];

        return older.Concat(range).OrderBy(x => x.Id).ToList();
    }

    public async Task<IReadOnlyList<MessageSearchCandidate>> Search(long conversationId, string pattern, int take,
        CancellationToken cancellationToken)
    {
        var found = await applicationDbContext.Messages
            .AsNoTracking()
            .Where(x => x.ConversationId == conversationId && x.Type == MessageType.Text && x.DeletedAt == null)
            .Where(x => EF.Functions.ILike(x.Text, pattern, "\\")
                        || x.Attachments.Any(a => EF.Functions.ILike(a.FileName, pattern, "\\"))
                        || x.Mentions.Any(m => EF.Functions.ILike(m.Employee.Name + " " + m.Employee.Surname, pattern, "\\")
                                               || EF.Functions.ILike(m.Employee.Surname + " " + m.Employee.Name, pattern, "\\")))
            .OrderByDescending(x => x.Id)
            .Take(take)
            .Select(x => new
            {
                x.Id,
                x.Text,
                FileNames = x.Attachments.Select(a => a.FileName).ToList(),
                Mentions = x.Mentions.Select(m => new { m.EmployeeId, m.Employee.Name, m.Employee.Surname }).ToList()
            })
            .ToListAsync(cancellationToken);

        // As the client shows a mention: "@Олена Коваль".
        return found
            .Select(x => new MessageSearchCandidate(
                x.Id,
                x.Text,
                x.FileNames,
                x.Mentions.GroupBy(m => m.EmployeeId).ToDictionary(g => g.Key, g => $"{g.First().Name} {g.First().Surname}".Trim())))
            .ToList();
    }

    public Task<Message> GetById(long id, CancellationToken cancellationToken)
    {
        return WithDetails(applicationDbContext.Messages.AsNoTracking())
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Message> GetForUpdate(long id, CancellationToken cancellationToken)
    {
        return WithDetails(applicationDbContext.Messages)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Message> FindByClientId(long authorId, Guid clientMessageId, CancellationToken cancellationToken)
    {
        return WithDetails(applicationDbContext.Messages.AsNoTracking())
            .FirstOrDefaultAsync(x => x.AuthorId == authorId && x.ClientMessageId == clientMessageId, cancellationToken);
    }

    public Task<bool> Exists(long conversationId, long messageId, CancellationToken cancellationToken)
    {
        return applicationDbContext.Messages
            .AnyAsync(x => x.Id == messageId && x.ConversationId == conversationId, cancellationToken);
    }

    public async Task Add(Message message, CancellationToken cancellationToken)
    {
        applicationDbContext.Messages.Add(message);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task Save(CancellationToken cancellationToken)
    {
        return applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    private static IQueryable<Message> WithDetails(IQueryable<Message> query) =>
        query
            .Include(x => x.Author)
            .Include(x => x.Conversation)
            .Include(x => x.ReplyToMessage)
            .ThenInclude(x => x.Author)
            .Include(x => x.ReplyToMessage)
            .ThenInclude(x => x.Mentions)
            .ThenInclude(x => x.Employee)
            .Include(x => x.Attachments)
            .Include(x => x.Mentions)
            .ThenInclude(x => x.Employee)
            .AsSplitQuery();
}
