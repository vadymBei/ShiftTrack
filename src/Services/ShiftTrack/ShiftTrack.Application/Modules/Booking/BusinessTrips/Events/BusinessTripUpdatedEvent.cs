using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Events;

public record BusinessTripUpdatedEvent(
    long BusinessTripId,
    IReadOnlyCollection<long> OldParticipantIds,
    IReadOnlyList<string> OldLocationIntegrationIds,
    DateTime OldStartDate,
    DateTime OldEndDate) : IDomainEvent;
