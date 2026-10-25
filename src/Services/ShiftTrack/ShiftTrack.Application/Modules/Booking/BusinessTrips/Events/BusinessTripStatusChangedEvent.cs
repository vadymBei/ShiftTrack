using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Events;

public record BusinessTripStatusChangedEvent(
    long BusinessTripId,
    BusinessTripStatus OldStatus,
    BusinessTripStatus NewStatus) : IDomainEvent;
