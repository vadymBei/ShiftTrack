using ShiftTrack.Application.Modules.Communication.Notifications.ViewModels;

namespace ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;

/// <summary>
/// Real-time delivery of notifications to connected clients (implemented with SignalR in the API).
/// </summary>
public interface INotificationPublisher
{
    Task NotificationCreated(long employeeId, NotificationVm notification, CancellationToken cancellationToken);
    Task NotificationsChanged(IReadOnlyCollection<long> employeeIds, CancellationToken cancellationToken);
}
