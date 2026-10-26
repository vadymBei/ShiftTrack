using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.ViewModels;

namespace ShiftTrack.Infrastructure.Realtime;

public class SignalRNotificationPublisher(
    IHubContext<NotificationsHub> hubContext,
    ILogger<SignalRNotificationPublisher> logger) : INotificationPublisher
{
    public async Task NotificationCreated(long employeeId, NotificationVm notification,
        CancellationToken cancellationToken)
    {
        try
        {
            await hubContext.Clients
                .Group(NotificationsHub.EmployeeGroup(employeeId))
                .SendAsync("notificationReceived", notification, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Failed to push notification {NotificationId}", notification.Id);
        }
    }

    public async Task NotificationsChanged(IReadOnlyCollection<long> employeeIds,
        CancellationToken cancellationToken)
    {
        if (employeeIds.Count == 0)
            return;

        try
        {
            await hubContext.Clients
                .Groups(employeeIds.Distinct().Select(NotificationsHub.EmployeeGroup).ToList())
                .SendAsync("notificationsChanged", cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Failed to push notifications change");
        }
    }
}
