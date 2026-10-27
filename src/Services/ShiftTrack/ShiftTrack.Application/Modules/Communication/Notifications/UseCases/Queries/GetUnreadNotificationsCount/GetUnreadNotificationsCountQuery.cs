using ShiftTrack.Application.Modules.Communication.Notifications.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Queries.GetUnreadNotificationsCount;

public record GetUnreadNotificationsCountQuery : IRequest<UnreadNotificationsCountVm>;
