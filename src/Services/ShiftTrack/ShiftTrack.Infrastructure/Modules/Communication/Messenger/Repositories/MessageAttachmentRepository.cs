using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Modules.Communication.Messenger.Repositories;

public class MessageAttachmentRepository(
    IApplicationDbContext applicationDbContext) : IMessageAttachmentRepository
{
    public async Task Add(MessageAttachment attachment, CancellationToken cancellationToken)
    {
        applicationDbContext.MessageAttachments.Add(attachment);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<MessageAttachment> GetById(long id, CancellationToken cancellationToken)
    {
        return applicationDbContext.MessageAttachments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<MessageAttachment>> GetUnsent(long authorId, long conversationId,
        IReadOnlyCollection<long> ids, CancellationToken cancellationToken)
    {
        return await applicationDbContext.MessageAttachments
            .Where(x => ids.Contains(x.Id)
                        && x.AuthorId == authorId
                        && x.ConversationId == conversationId
                        && x.MessageId == null)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MessageAttachment>> GetUnsentBefore(DateTime createdBefore, int take,
        CancellationToken cancellationToken)
    {
        return await applicationDbContext.MessageAttachments
            .Where(x => x.MessageId == null && x.CreatedAt < createdBefore)
            .OrderBy(x => x.CreatedAt)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetStoredNames(long conversationId, CancellationToken cancellationToken)
    {
        return await applicationDbContext.MessageAttachments
            .AsNoTracking()
            .Where(x => x.ConversationId == conversationId)
            .Select(x => x.StoredName)
            .ToListAsync(cancellationToken);
    }

    public async Task Remove(IReadOnlyCollection<MessageAttachment> attachments, CancellationToken cancellationToken)
    {
        if (attachments.Count == 0)
            return;

        applicationDbContext.MessageAttachments.RemoveRange(attachments);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
