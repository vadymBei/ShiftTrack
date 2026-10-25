using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Domain.Modules.Communication.Notifications.Entities;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;

public interface INotificationRepository
{
    Task AddRange(IReadOnlyCollection<Notification> notifications, CancellationToken cancellationToken);
    Task<Notification> GetById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Notification>> GetForEmployee(long employeeId, NotificationsFilterDto filter, CancellationToken cancellationToken);
    Task<int> CountUnread(long employeeId, CancellationToken cancellationToken);
    Task MarkAsRead(long id, CancellationToken cancellationToken);
    Task<int> MarkAllAsRead(long employeeId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<long>> MarkAsReadByEntity(
        NotificationModuleType moduleType,
        long entityId,
        IReadOnlyCollection<NotificationType> types,
        CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
}
