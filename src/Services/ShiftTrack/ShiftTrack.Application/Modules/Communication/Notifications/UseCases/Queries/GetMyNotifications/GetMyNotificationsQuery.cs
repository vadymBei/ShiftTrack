using ShiftTrack.Application.Modules.Communication.Notifications.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Queries.GetMyNotifications;

public record GetMyNotificationsQuery(
    int Take,
    bool OnlyUnread) : IRequest<IEnumerable<NotificationVm>>;
