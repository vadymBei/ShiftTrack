using AutoMapper;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Constants;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.Exceptions;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.ViewModels;
using ShiftTrack.Domain.Modules.Communication.Notifications.Entities;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.Services;

public class NotificationService(
    IMapper mapper,
    ICurrentUserService currentUserService,
    INotificationRepository notificationRepository,
    INotificationPublisher notificationPublisher) : INotificationService
{
    private static readonly NotificationType[] PendingApprovalTypes =
    [
        NotificationType.VacationPendingApproval,
        NotificationType.BusinessTripPendingApproval
    ];

    public async Task Notify(NotificationToCreateDto dto, IEnumerable<long> recipientIds,
        CancellationToken cancellationToken)
    {
        var sender = currentUserService.Employee;

        var recipients = recipientIds
            .Where(id => id > 0 && id != sender?.Id)
            .Distinct()
            .ToList();

        if (recipients.Count == 0)
            return;

        var notifications = recipients
            .Select(employeeId => new Notification
            {
                EmployeeId = employeeId,
                Type = dto.Type,
                ModuleType = dto.ModuleType,
                EntityId = dto.EntityId,
                Title = dto.Title,
                Message = dto.Message,
                ExpiresAt = dto.ExpiresAt
            })
            .ToList();

        await notificationRepository.AddRange(notifications, cancellationToken);

        foreach (var notification in notifications)
        {
            var notificationVm = mapper.Map<NotificationVm>(notification);
            notificationVm.AuthorFullName = sender?.FullName;

            await notificationPublisher.NotificationCreated(notification.EmployeeId, notificationVm, cancellationToken);
        }
    }

    public async Task ResolvePendingApprovals(NotificationModuleType moduleType, long entityId,
        CancellationToken cancellationToken)
    {
        var employeeIds = await notificationRepository
            .MarkAsReadByEntity(moduleType, entityId, PendingApprovalTypes, cancellationToken);

        if (employeeIds.Count > 0)
            await notificationPublisher.NotificationsChanged(employeeIds, cancellationToken);
    }

    public Task<IEnumerable<Notification>> GetMy(NotificationsFilterDto filter, CancellationToken cancellationToken)
    {
        var take = Math.Clamp(filter.Take, 1, 100);

        return notificationRepository.GetForEmployee(
            currentUserService.Employee.Id,
            filter with { Take = take },
            cancellationToken);
    }

    public Task<int> CountMyUnread(CancellationToken cancellationToken)
    {
        return notificationRepository.CountUnread(currentUserService.Employee.Id, cancellationToken);
    }

    public async Task MarkAsRead(long id, CancellationToken cancellationToken)
    {
        var notification = await GetOwn(id, cancellationToken);

        if (notification.ReadAt is not null)
            return;

        await notificationRepository.MarkAsRead(notification.Id, cancellationToken);
        await notificationPublisher.NotificationsChanged([notification.EmployeeId], cancellationToken);
    }

    public async Task MarkAllAsRead(CancellationToken cancellationToken)
    {
        var employeeId = currentUserService.Employee.Id;

        var updated = await notificationRepository.MarkAllAsRead(employeeId, cancellationToken);

        if (updated > 0)
            await notificationPublisher.NotificationsChanged([employeeId], cancellationToken);
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var notification = await GetOwn(id, cancellationToken);

        await notificationRepository.Delete(notification.Id, cancellationToken);
        await notificationPublisher.NotificationsChanged([notification.EmployeeId], cancellationToken);
    }

    private async Task<Notification> GetOwn(long id, CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.GetById(id, cancellationToken);

        if (notification.EmployeeId != currentUserService.Employee.Id)
        {
            throw new NotificationException(
                NotificationExceptionsLocalization.CANNOT_ACCESS_OTHERS_NOTIFICATION,
                nameof(NotificationExceptionsLocalization.CANNOT_ACCESS_OTHERS_NOTIFICATION));
        }

        return notification;
    }
}
