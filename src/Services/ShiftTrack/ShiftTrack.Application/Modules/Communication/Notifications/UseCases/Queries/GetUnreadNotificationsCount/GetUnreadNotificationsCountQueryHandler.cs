using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Queries.GetUnreadNotificationsCount;

public class GetUnreadNotificationsCountQueryHandler(
    INotificationService notificationService)
    : IRequestHandler<GetUnreadNotificationsCountQuery, UnreadNotificationsCountVm>
{
    public async Task<UnreadNotificationsCountVm> Handle(GetUnreadNotificationsCountQuery request,
        CancellationToken cancellationToken = default)
    {
        return new UnreadNotificationsCountVm
        {
            Count = await notificationService.CountMyUnread(cancellationToken)
        };
    }
}
