using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Events;

public record BusinessTripDeletedEvent(
    long BusinessTripId,
    long? AuthorId,
    IReadOnlyCollection<long> ParticipantIds,
    IReadOnlyList<string> LocationIntegrationIds,
    DateTime StartDate,
    DateTime EndDate) : IDomainEvent;
