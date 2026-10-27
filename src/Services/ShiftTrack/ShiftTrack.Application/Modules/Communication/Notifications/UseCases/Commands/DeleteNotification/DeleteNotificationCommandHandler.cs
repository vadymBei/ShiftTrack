using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Commands.DeleteNotification;

public class DeleteNotificationCommandHandler(
    INotificationService notificationService) : IRequestHandler<DeleteNotificationCommand>
{
    public Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken = default)
    {
        return notificationService.Delete(request.Id, cancellationToken);
    }
}
