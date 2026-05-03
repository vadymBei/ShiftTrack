using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Dtos;

public record VacationsFilterDto(
    DateTime? StartDate,
    DateTime? EndDate,
    long UnitId,
    long DepartmentId,
    VacationStatus Status,
    string SearchPattern);