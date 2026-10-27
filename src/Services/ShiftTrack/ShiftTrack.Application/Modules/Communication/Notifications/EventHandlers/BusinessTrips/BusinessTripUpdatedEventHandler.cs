using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Events;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Services;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.EventHandlers.BusinessTrips;

public class BusinessTripUpdatedEventHandler(
    ILogger<BusinessTripUpdatedEventHandler> logger,
    IBusinessTripRepository businessTripRepository,
    IBusinessTripRouteService businessTripRouteService,
    INotificationService notificationService) : NotificationEventHandler<BusinessTripUpdatedEvent>(logger)
{
    protected override async Task HandleEvent(BusinessTripUpdatedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(domainEvent.BusinessTripId, cancellationToken);

        var locationIds = (businessTrip.Locations ?? [])
            .OrderBy(x => x.Id)
            .Select(x => x.LocationIntegrationId)
            .ToList();

        var route = await businessTripRouteService.GetRoute(locationIds, cancellationToken)
                    ?? NotificationTextFormatter.BusinessTripFallbackRoute;
        var period = NotificationTextFormatter.Period(businessTrip.StartDate, businessTrip.EndDate);
        var summary = $"{route}, {period}";

        var newParticipantIds = (businessTrip.Participants ?? []).Select(x => x.Id).ToHashSet();
        var oldParticipantIds = domainEvent.OldParticipantIds.ToHashSet();

        var added = newParticipantIds.Except(oldParticipantIds).ToList();
        var removed = oldParticipantIds.Except(newParticipantIds).ToList();
        var stayed = newParticipantIds.Intersect(oldParticipantIds).ToList();

        await notificationService.Notify(
            new NotificationToCreateDto(
                NotificationType.BusinessTripAdded,
                NotificationModuleType.BusinessTrip,
                businessTrip.Id,
                "Вас додано до відрядження",
                summary),
            added,
            cancellationToken);

        await notificationService.Notify(
            new NotificationToCreateDto(
                NotificationType.BusinessTripRemoved,
                NotificationModuleType.BusinessTrip,
                businessTrip.Id,
                "Вас прибрали з відрядження",
                summary),
            removed,
            cancellationToken);

        var datesChanged = businessTrip.StartDate.Date != domainEvent.OldStartDate.Date
                           || businessTrip.EndDate.Date != domainEvent.OldEndDate.Date;
        var routeChanged = !locationIds.SequenceEqual(domainEvent.OldLocationIntegrationIds);

        if (!datesChanged && !routeChanged)
            return;

        var message = datesChanged
            ? $"{summary} (було {NotificationTextFormatter.Period(domainEvent.OldStartDate, domainEvent.OldEndDate)})"
            : summary;

        await notificationService.Notify(
            new NotificationToCreateDto(
                NotificationType.BusinessTripChanged,
                NotificationModuleType.BusinessTrip,
                businessTrip.Id,
                routeChanged ? "Змінено маршрут відрядження" : "Змінено дати відрядження",
                message),
            stayed,
            cancellationToken);
    }
}
