using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Commands.MarkNotificationAsRead;

public class MarkNotificationAsReadCommandHandler(
    INotificationService notificationService) : IRequestHandler<MarkNotificationAsReadCommand>
{
    public Task Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken = default)
    {
        return notificationService.MarkAsRead(request.Id, cancellationToken);
    }
}
