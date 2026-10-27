using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Commands.MarkAllNotificationsAsRead;

public class MarkAllNotificationsAsReadCommandHandler(
    INotificationService notificationService) : IRequestHandler<MarkAllNotificationsAsReadCommand>
{
    public Task Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken = default)
    {
        return notificationService.MarkAllAsRead(cancellationToken);
    }
}
