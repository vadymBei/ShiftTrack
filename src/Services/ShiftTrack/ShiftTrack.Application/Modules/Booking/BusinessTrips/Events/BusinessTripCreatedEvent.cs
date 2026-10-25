using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Events;

public record BusinessTripCreatedEvent(
    long BusinessTripId) : IDomainEvent;
