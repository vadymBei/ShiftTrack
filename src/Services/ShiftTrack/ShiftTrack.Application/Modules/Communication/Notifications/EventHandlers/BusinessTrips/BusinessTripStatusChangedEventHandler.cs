using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Events;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Services;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Enums;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.EventHandlers.BusinessTrips;

public class BusinessTripStatusChangedEventHandler(
    ILogger<BusinessTripStatusChangedEventHandler> logger,
    IBusinessTripRepository businessTripRepository,
    IBusinessTripRouteService businessTripRouteService,
    INotificationService notificationService) : NotificationEventHandler<BusinessTripStatusChangedEvent>(logger)
{
    protected override async Task HandleEvent(BusinessTripStatusChangedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        if (domainEvent.NewStatus is not (BusinessTripStatus.Approved or BusinessTripStatus.Rejected))
            return;

        await notificationService.ResolvePendingApprovals(
            NotificationModuleType.BusinessTrip, domainEvent.BusinessTripId, cancellationToken);

        var businessTrip = await businessTripRepository.GetById(domainEvent.BusinessTripId, cancellationToken);

        var route = await businessTripRouteService.GetRoute(businessTrip, cancellationToken)
                    ?? NotificationTextFormatter.BusinessTripFallbackRoute;

        var summary = $"{route}, {NotificationTextFormatter.Period(businessTrip.StartDate, businessTrip.EndDate)}";

        var recipients = (businessTrip.Participants ?? []).Select(x => x.Id).ToList();

        if (businessTrip.AuthorId is { } authorId)
            recipients.Add(authorId);

        var approved = domainEvent.NewStatus == BusinessTripStatus.Approved;

        await notificationService.Notify(
            new NotificationToCreateDto(
                approved ? NotificationType.BusinessTripApproved : NotificationType.BusinessTripRejected,
                NotificationModuleType.BusinessTrip,
                businessTrip.Id,
                approved ? "Відрядження погоджено" : "Відрядження відхилено",
                approved ? $"{summary}. Наказ можна завантажити." : summary),
            recipients,
            cancellationToken);
    }
}
