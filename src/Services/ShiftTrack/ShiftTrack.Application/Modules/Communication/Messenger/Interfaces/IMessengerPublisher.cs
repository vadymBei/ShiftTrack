using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

/// <summary>Real-time delivery of chat events to the members' clients (SignalR).</summary>
public interface IMessengerPublisher
{
    Task MessageCreated(IReadOnlyCollection<long> employeeIds, MessageVm message, CancellationToken cancellationToken);

    /// <summary>A message was edited or deleted.</summary>
    Task MessageUpdated(IReadOnlyCollection<long> employeeIds, MessageVm message, CancellationToken cancellationToken);

    /// <summary>The conversation was created, renamed or its members changed: clients reload it.</summary>
    Task ConversationChanged(IReadOnlyCollection<long> employeeIds, long conversationId, CancellationToken cancellationToken);

    Task ConversationRead(IReadOnlyCollection<long> employeeIds, ConversationReadVm read, CancellationToken cancellationToken);

    Task Typing(IReadOnlyCollection<long> employeeIds, TypingVm typing, CancellationToken cancellationToken);
}
