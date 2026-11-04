using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

public interface IConversationService
{
    Task<IReadOnlyList<ConversationSummary>> GetMy(int take, CancellationToken cancellationToken);
    Task<ConversationSummary> Get(long id, CancellationToken cancellationToken);
    Task<ConversationSummary> OpenDirect(long employeeId, CancellationToken cancellationToken);
    Task<ConversationSummary> CreateGroup(string title, IReadOnlyCollection<long> memberIds, CancellationToken cancellationToken);
    Task<ConversationSummary> Rename(long id, string title, CancellationToken cancellationToken);
    Task<ConversationSummary> AddMembers(long id, IReadOnlyCollection<long> employeeIds, CancellationToken cancellationToken);
    Task<ConversationSummary> RemoveMember(long id, long employeeId, CancellationToken cancellationToken);
    Task Leave(long id, CancellationToken cancellationToken);
    Task MarkRead(long id, long messageId, CancellationToken cancellationToken);
    Task<ConversationSummary> SetMuted(long id, bool muted, CancellationToken cancellationToken);
    Task Typing(long id, CancellationToken cancellationToken);
    Task<MessengerUnreadCount> CountUnread(CancellationToken cancellationToken);

    /// <summary>Employees of the whole company to start a chat with, except the current user.</summary>
    Task<IReadOnlyList<Employee>> SearchContacts(string search, CancellationToken cancellationToken);
}
