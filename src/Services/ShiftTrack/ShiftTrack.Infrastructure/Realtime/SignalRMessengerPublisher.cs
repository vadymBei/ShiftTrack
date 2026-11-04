using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;

namespace ShiftTrack.Infrastructure.Realtime;

/// <summary>
/// Chat events go through the notifications hub to the members' employee groups:
/// "chatMessageCreated", "chatMessageUpdated", "chatConversationChanged", "chatConversationRead", "chatTyping".
/// </summary>
public class SignalRMessengerPublisher(
    IHubContext<NotificationsHub> hubContext,
    ILogger<SignalRMessengerPublisher> logger) : IMessengerPublisher
{
    public Task MessageCreated(IReadOnlyCollection<long> employeeIds, MessageVm message,
        CancellationToken cancellationToken) =>
        Send(employeeIds, "chatMessageCreated", message, cancellationToken);

    public Task MessageUpdated(IReadOnlyCollection<long> employeeIds, MessageVm message,
        CancellationToken cancellationToken) =>
        Send(employeeIds, "chatMessageUpdated", message, cancellationToken);

    public Task ConversationChanged(IReadOnlyCollection<long> employeeIds, long conversationId,
        CancellationToken cancellationToken) =>
        Send(employeeIds, "chatConversationChanged", conversationId, cancellationToken);

    public Task ConversationRead(IReadOnlyCollection<long> employeeIds, ConversationReadVm read,
        CancellationToken cancellationToken) =>
        Send(employeeIds, "chatConversationRead", read, cancellationToken);

    public Task Typing(IReadOnlyCollection<long> employeeIds, TypingVm typing, CancellationToken cancellationToken) =>
        Send(employeeIds, "chatTyping", typing, cancellationToken);

    private async Task Send(IReadOnlyCollection<long> employeeIds, string method, object payload,
        CancellationToken cancellationToken)
    {
        if (employeeIds.Count == 0)
            return;

        try
        {
            await hubContext.Clients
                .Groups(employeeIds.Distinct().Select(NotificationsHub.EmployeeGroup).ToList())
                .SendAsync(method, payload, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Failed to push {Method}", method);
        }
    }
}
