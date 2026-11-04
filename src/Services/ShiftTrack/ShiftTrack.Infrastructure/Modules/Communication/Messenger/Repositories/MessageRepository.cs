using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
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
