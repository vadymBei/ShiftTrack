namespace ShiftTrack.Domain.Features.Booking.Vacations.Enums;

public enum VacationStatus
{
    None,
    PendingApproval,
    ApprovedByDepartmentDirector,
    ApprovedByUnitDirector,
    Rejected
}