namespace ShiftTrack.Application.Modules.Booking.Vacations.Dtos;

/// <summary>
/// Vacations the current user may see, applied in the database query.
/// A vacation is visible if its employee works in <see cref="UnitIds"/> / <see cref="DepartmentIds"/>,
/// or <see cref="OwnerId"/> is its employee or author. All values null means no restriction.
/// </summary>
public record VacationAccessFilterDto(
    IReadOnlyCollection<long> UnitIds,
    IReadOnlyCollection<long> DepartmentIds,
    long? OwnerId)
{
    public static readonly VacationAccessFilterDto Unrestricted = new(null, null, null);

    public bool IsUnrestricted => UnitIds is null && DepartmentIds is null && OwnerId is null;
}
