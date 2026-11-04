using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

public interface IMessageRepository
{
    /// <summary>Messages older than <paramref name="beforeId"/> (or the newest), in chronological order.</summary>
    Task<IReadOnlyList<Message>> GetPage(long conversationId, long? beforeId, int take, CancellationToken cancellationToken);
    /// <summary>
    /// Messages before <paramref name="beforeId"/> down to <paramref name="untilId"/> (at most <paramref name="maxRange"/>),
    /// and <paramref name="take"/> more before it, oldest first.
    /// </summary>
    Task<IReadOnlyList<Message>> GetPageUntil(long conversationId, long? beforeId, long untilId, int take, int maxRange,
        CancellationToken cancellationToken);
    /// <summary>
    /// The newest text messages (not deleted) whose text, file names or mentioned names match the LIKE
    /// <paramref name="pattern"/> ("\\" escapes), case-insensitively.
    /// </summary>
    Task<IReadOnlyList<MessageSearchCandidate>> Search(long conversationId, string pattern, int take,
        CancellationToken cancellationToken);

    /// <summary>With the author and the replied message; null if not found.</summary>
    Task<Message> GetById(long id, CancellationToken cancellationToken);

    /// <summary>Tracked, with the author and the replied message; null if not found.</summary>
    Task<Message> GetForUpdate(long id, CancellationToken cancellationToken);

    Task<Message> FindByClientId(long authorId, Guid clientMessageId, CancellationToken cancellationToken);

    Task<bool> Exists(long conversationId, long messageId, CancellationToken cancellationToken);

    Task Add(Message message, CancellationToken cancellationToken);

    Task Save(CancellationToken cancellationToken);
}
