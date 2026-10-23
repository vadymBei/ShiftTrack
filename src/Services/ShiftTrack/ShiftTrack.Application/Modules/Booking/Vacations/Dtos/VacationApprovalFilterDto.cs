using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Dtos;

public record VacationApprovalFilterDto(
    IReadOnlyCollection<VacationStatus> Statuses,
    long? UnitId,
    long? DepartmentId,
    long? ExcludeEmployeeId);
