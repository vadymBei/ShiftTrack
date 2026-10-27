using AutoMapper;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Queries.GetMyNotifications;

public class GetMyNotificationsQueryHandler(
    IMapper mapper,
    INotificationService notificationService) : IRequestHandler<GetMyNotificationsQuery, IEnumerable<NotificationVm>>
{
    public async Task<IEnumerable<NotificationVm>> Handle(GetMyNotificationsQuery request,
        CancellationToken cancellationToken = default)
    {
        var notifications = await notificationService.GetMy(
            new NotificationsFilterDto(request.Take, request.OnlyUnread),
            cancellationToken);

        return mapper.Map<IEnumerable<NotificationVm>>(notifications);
    }
}
