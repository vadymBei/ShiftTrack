namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;

/// <summary>
/// Business trips the current user may see, applied in the database query.
/// A trip is visible if a participant works in <see cref="ParticipantDepartmentIds"/>,
/// or <see cref="OwnerId"/> is its author or a participant. All values null means no restriction.
/// </summary>
public record BusinessTripAccessFilterDto(
    IReadOnlyCollection<long> ParticipantDepartmentIds,
    long? OwnerId)
{
    public static readonly BusinessTripAccessFilterDto Unrestricted = new(null, null);

    public bool IsUnrestricted => ParticipantDepartmentIds is null && OwnerId is null;
}
