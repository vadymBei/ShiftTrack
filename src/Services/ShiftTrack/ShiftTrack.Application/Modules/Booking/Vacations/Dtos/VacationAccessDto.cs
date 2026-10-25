namespace ShiftTrack.Application.Modules.Booking.Vacations.Dtos;

/// <summary>
/// What the current user's strategy allows for a vacation.
/// The same values are used to validate actions and to tell the client which actions to show.
/// </summary>
public record VacationAccessDto(
    bool CanView,
    bool CanApprove,
    bool CanReject,
    bool ApproveSubmitsForApproval)
{
    public static readonly VacationAccessDto None = new(false, false, false, false);
}
