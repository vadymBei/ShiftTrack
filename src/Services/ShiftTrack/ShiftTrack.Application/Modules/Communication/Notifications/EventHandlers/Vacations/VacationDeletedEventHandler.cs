using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Booking.Vacations.Events;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Services;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.EventHandlers.Vacations;

public class VacationDeletedEventHandler(
    ILogger<VacationDeletedEventHandler> logger,
    INotificationService notificationService) : NotificationEventHandler<VacationDeletedEvent>(logger)
{
    protected override async Task HandleEvent(VacationDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        await notificationService.ResolvePendingApprovals(
            NotificationModuleType.Vacation, domainEvent.VacationId, cancellationToken);

        var recipients = new List<long> { domainEvent.EmployeeId };

        if (domainEvent.AuthorId is { } authorId)
            recipients.Add(authorId);

        await notificationService.Notify(
            new NotificationToCreateDto(
                NotificationType.VacationDeleted,
                NotificationModuleType.Vacation,
                domainEvent.VacationId,
                "Заяву на відпустку видалено",
                $"{NotificationTextFormatter.VacationType(domainEvent.Type)}, " +
                $"{NotificationTextFormatter.Period(domainEvent.StartDate, domainEvent.EndDate)}"),
            recipients,
            cancellationToken);
    }
}
