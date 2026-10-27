using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Dtos;

/// <param name="UnitIds">Whole units in scope; together with <paramref name="DepartmentIds"/> null means no restriction.</param>
/// <param name="DepartmentIds">Separate departments in scope.</param>
public record VacationApprovalFilterDto(
    IReadOnlyCollection<VacationStatus> Statuses,
    IReadOnlyCollection<long> UnitIds,
    IReadOnlyCollection<long> DepartmentIds,
    long? ExcludeEmployeeId);
