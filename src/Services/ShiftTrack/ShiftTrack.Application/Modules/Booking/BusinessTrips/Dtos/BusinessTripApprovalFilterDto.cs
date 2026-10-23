namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;

public record BusinessTripApprovalFilterDto(
    IReadOnlyCollection<long> ParticipantDepartmentIds,
    long? ExcludeParticipantId);
