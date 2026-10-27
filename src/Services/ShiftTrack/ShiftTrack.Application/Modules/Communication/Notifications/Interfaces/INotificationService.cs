using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Domain.Modules.Communication.Notifications.Entities;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;

public interface INotificationService
{
    Task Notify(NotificationToCreateDto dto, IEnumerable<long> recipientIds, CancellationToken cancellationToken);
    Task ResolvePendingApprovals(NotificationModuleType moduleType, long entityId, CancellationToken cancellationToken);
    Task<IEnumerable<Notification>> GetMy(NotificationsFilterDto filter, CancellationToken cancellationToken);
    Task<int> CountMyUnread(CancellationToken cancellationToken);
    Task MarkAsRead(long id, CancellationToken cancellationToken);
    Task MarkAllAsRead(CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
}
