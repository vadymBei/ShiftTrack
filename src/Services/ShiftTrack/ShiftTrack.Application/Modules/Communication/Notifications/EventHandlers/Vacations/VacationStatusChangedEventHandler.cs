using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Booking.Vacations.Events;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Services;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.EventHandlers.Vacations;

public class VacationStatusChangedEventHandler(
    ILogger<VacationStatusChangedEventHandler> logger,
    IVacationRepository vacationRepository,
    INotificationService notificationService,
    INotificationRecipientResolver recipientResolver) : NotificationEventHandler<VacationStatusChangedEvent>(logger)
{
    protected override async Task HandleEvent(VacationStatusChangedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(domainEvent.VacationId, cancellationToken);

        var summary = $"{NotificationTextFormatter.VacationType(vacation.Type)}, " +
                      $"{NotificationTextFormatter.Period(vacation.StartDate, vacation.EndDate)}";

        switch (domainEvent.NewStatus)
        {
            case VacationStatus.PendingApproval:
                await NotifyFirstApprovers(vacation, summary, cancellationToken);
                break;

            case VacationStatus.ApprovedByDepartmentDirector:
                await notificationService.ResolvePendingApprovals(
                    NotificationModuleType.Vacation, vacation.Id, cancellationToken);

                await NotifyOwners(vacation, NotificationType.VacationApproved,
                    "Відпустку погоджено директором",
                    $"{summary}. Заява чекає на регіонального директора.",
                    cancellationToken);

                await NotifyUnitDirectors(vacation,
                    "Заява на відпустку чекає вашого рішення",
                    $"{vacation.Employee.FullName} \u00b7 {summary}. Погоджено директором магазину.",
                    cancellationToken);
                break;

            case VacationStatus.ApprovedByUnitDirector:
                await notificationService.ResolvePendingApprovals(
                    NotificationModuleType.Vacation, vacation.Id, cancellationToken);

                await NotifyOwners(vacation, NotificationType.VacationFinallyApproved,
                    "Відпустку погоджено",
                    $"{summary}. Заяву можна завантажити.",
                    cancellationToken);
                break;

            case VacationStatus.Rejected:
                await notificationService.ResolvePendingApprovals(
                    NotificationModuleType.Vacation, vacation.Id, cancellationToken);

                await NotifyOwners(vacation, NotificationType.VacationRejected,
                    "Заяву на відпустку відхилено",
                    summary,
                    cancellationToken);
                break;
        }
    }

    private async Task NotifyFirstApprovers(Vacation vacation, string summary, CancellationToken cancellationToken)
    {
        var departmentId = vacation.Employee?.DepartmentId;

        List<long> recipients = departmentId is null
            ? []
            : (await recipientResolver.GetDepartmentDirectors(departmentId.Value, cancellationToken))
            .Where(id => id != vacation.EmployeeId)
            .ToList();

        if (recipients.Count == 0)
        {
            await NotifyUnitDirectors(vacation,
                "Нова заява на відпустку",
                $"{vacation.Employee?.FullName} \u00b7 {summary}",
                cancellationToken);
            return;
        }

        await notificationService.Notify(
            new NotificationToCreateDto(
                NotificationType.VacationPendingApproval,
                NotificationModuleType.Vacation,
                vacation.Id,
                "Нова заява на відпустку",
                $"{vacation.Employee?.FullName} \u00b7 {summary}"),
            recipients,
            cancellationToken);
    }

    private async Task NotifyUnitDirectors(Vacation vacation, string title, string message,
        CancellationToken cancellationToken)
    {
        var departmentId = vacation.Employee?.DepartmentId;

        if (departmentId is null)
            return;

        var recipients = await recipientResolver.GetUnitDirectors(departmentId.Value, cancellationToken);

        await notificationService.Notify(
            new NotificationToCreateDto(
                NotificationType.VacationPendingApproval,
                NotificationModuleType.Vacation,
                vacation.Id,
                title,
                message),
            recipients,
            cancellationToken);
    }

    private async Task NotifyOwners(Vacation vacation, NotificationType type, string title, string message,
        CancellationToken cancellationToken)
    {
        await notificationService.Notify(
            new NotificationToCreateDto(type, NotificationModuleType.Vacation, vacation.Id, title, message),
            [vacation.EmployeeId],
            cancellationToken);

        if (vacation.AuthorId is { } authorId && authorId != vacation.EmployeeId)
        {
            await notificationService.Notify(
                new NotificationToCreateDto(type, NotificationModuleType.Vacation, vacation.Id, title,
                    $"{vacation.Employee?.FullName} \u00b7 {message}"),
                [authorId],
                cancellationToken);
        }
    }
}
