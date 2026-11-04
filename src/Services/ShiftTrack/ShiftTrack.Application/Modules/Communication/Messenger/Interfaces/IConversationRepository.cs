using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

public interface IConversationRepository
{
    /// <summary>Conversations of the employee, recent first; one conversation when <paramref name="conversationId"/> is set.</summary>
    Task<IReadOnlyList<ConversationSummary>> GetSummaries(long employeeId, int take, long? conversationId,
        CancellationToken cancellationToken);

    /// <summary>Tracked conversation with members and their employees; null if not found.</summary>
    Task<Conversation> GetForUpdate(long id, CancellationToken cancellationToken);

    Task<bool> IsMember(long conversationId, long employeeId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<long>> GetMemberIds(long conversationId, CancellationToken cancellationToken);

    /// <summary>Id of the direct conversation of two employees, created if there is none.</summary>
    Task<long> GetOrCreateDirect(long employeeId, long otherEmployeeId, CancellationToken cancellationToken);

    Task Add(Conversation conversation, CancellationToken cancellationToken);

    /// <summary>Deletes a tracked conversation with its members and messages.</summary>
    Task Remove(Conversation conversation, CancellationToken cancellationToken);

    Task Save(CancellationToken cancellationToken);

    Task SetLastMessage(long conversationId, long messageId, DateTime sentAt, CancellationToken cancellationToken);

    /// <summary>Moves the read position forward; false if it is already there or further.</summary>
    Task<bool> MarkRead(long conversationId, long employeeId, long messageId, CancellationToken cancellationToken);

    /// <summary>Not muted conversations only.</summary>
    Task<MessengerUnreadCount> CountUnread(long employeeId, CancellationToken cancellationToken);

    /// <summary>False if it already was so.</summary>
    Task<bool> SetMuted(long conversationId, long employeeId, bool muted, CancellationToken cancellationToken);
}
