using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Events;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Services;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.EventHandlers.BusinessTrips;

public class BusinessTripDeletedEventHandler(
    ILogger<BusinessTripDeletedEventHandler> logger,
    IBusinessTripRouteService businessTripRouteService,
    INotificationService notificationService) : NotificationEventHandler<BusinessTripDeletedEvent>(logger)
{
    protected override async Task HandleEvent(BusinessTripDeletedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await notificationService.ResolvePendingApprovals(
            NotificationModuleType.BusinessTrip, domainEvent.BusinessTripId, cancellationToken);

        var route = await businessTripRouteService.GetRoute(domainEvent.LocationIntegrationIds, cancellationToken)
                    ?? NotificationTextFormatter.BusinessTripFallbackRoute;

        var recipients = domainEvent.ParticipantIds.ToList();

        if (domainEvent.AuthorId is { } authorId)
            recipients.Add(authorId);

        await notificationService.Notify(
            new NotificationToCreateDto(
                NotificationType.BusinessTripRemoved,
                NotificationModuleType.BusinessTrip,
                domainEvent.BusinessTripId,
                "Відрядження скасовано",
                $"{route}, {NotificationTextFormatter.Period(domainEvent.StartDate, domainEvent.EndDate)}"),
            recipients,
            cancellationToken);
    }
}
