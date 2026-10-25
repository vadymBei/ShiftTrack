using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.ViewModels;

namespace ShiftTrack.Jobs.Services;

/// <summary>
/// The jobs host has no real-time clients: notifications created here are stored
/// and the users get them on the next load.
/// </summary>
public class NullNotificationPublisher : INotificationPublisher
{
    public Task NotificationCreated(long employeeId, NotificationVm notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task NotificationsChanged(IReadOnlyCollection<long> employeeIds, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
