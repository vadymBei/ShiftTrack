using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Events;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Services;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.EventHandlers.BusinessTrips;

public class BusinessTripCreatedEventHandler(
    ILogger<BusinessTripCreatedEventHandler> logger,
    IBusinessTripRepository businessTripRepository,
    IBusinessTripRouteService businessTripRouteService,
    INotificationService notificationService,
    INotificationRecipientResolver recipientResolver) : NotificationEventHandler<BusinessTripCreatedEvent>(logger)
{
    protected override async Task HandleEvent(BusinessTripCreatedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(domainEvent.BusinessTripId, cancellationToken);

        var participants = (businessTrip.Participants ?? [])
            .OrderBy(x => x.Surname)
            .ThenBy(x => x.Name)
            .ToList();

        var route = await businessTripRouteService.GetRoute(businessTrip, cancellationToken)
                    ?? NotificationTextFormatter.BusinessTripFallbackRoute;

        var summary = $"{route}, {NotificationTextFormatter.Period(businessTrip.StartDate, businessTrip.EndDate)}";

        var approvers = await recipientResolver.GetUnitDirectorsForDepartments(
            participants.Where(x => x.DepartmentId is not null).Select(x => x.DepartmentId.Value),
            cancellationToken);

        await notificationService.Notify(
            new NotificationToCreateDto(
                NotificationType.BusinessTripPendingApproval,
                NotificationModuleType.BusinessTrip,
                businessTrip.Id,
                "Нова заявка на відрядження",
                $"{NotificationTextFormatter.Participants(participants.Select(x => x.FullName).ToList())} \u00b7 {summary}"),
            approvers,
            cancellationToken);

        await notificationService.Notify(
            new NotificationToCreateDto(
                NotificationType.BusinessTripAdded,
                NotificationModuleType.BusinessTrip,
                businessTrip.Id,
                "Вас додано до відрядження",
                summary),
            participants.Select(x => x.Id).Where(id => !approvers.Contains(id)),
            cancellationToken);
    }
}
