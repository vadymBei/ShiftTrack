using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Booking.Vacations.Events;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Services;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.EventHandlers.Vacations;

public class VacationCreatedEventHandler(
    ILogger<VacationCreatedEventHandler> logger,
    IVacationRepository vacationRepository,
    INotificationService notificationService) : NotificationEventHandler<VacationCreatedEvent>(logger)
{
    protected override async Task HandleEvent(VacationCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(domainEvent.VacationId, cancellationToken);

        if (vacation.AuthorId is null || vacation.AuthorId == vacation.EmployeeId)
            return;

        await notificationService.Notify(
            new NotificationToCreateDto(
                NotificationType.VacationCreatedForYou,
                NotificationModuleType.Vacation,
                vacation.Id,
                "На вас створено заяву на відпустку",
                $"{NotificationTextFormatter.VacationType(vacation.Type)}, " +
                $"{NotificationTextFormatter.Period(vacation.StartDate, vacation.EndDate)}"),
            [vacation.EmployeeId],
            cancellationToken);
    }
}
