using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

public interface IMessageAttachmentRepository
{
    Task Add(MessageAttachment attachment, CancellationToken cancellationToken);

    /// <summary>Null if not found.</summary>
    Task<MessageAttachment> GetById(long id, CancellationToken cancellationToken);

    /// <summary>Tracked uploads of the author in the conversation that are not sent yet.</summary>
    Task<IReadOnlyList<MessageAttachment>> GetUnsent(long authorId, long conversationId, IReadOnlyCollection<long> ids,
        CancellationToken cancellationToken);

    /// <summary>Unsent uploads created before the time, oldest first.</summary>
    Task<IReadOnlyList<MessageAttachment>> GetUnsentBefore(DateTime createdBefore, int take,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> GetStoredNames(long conversationId, CancellationToken cancellationToken);

    Task Remove(IReadOnlyCollection<MessageAttachment> attachments, CancellationToken cancellationToken);
}
