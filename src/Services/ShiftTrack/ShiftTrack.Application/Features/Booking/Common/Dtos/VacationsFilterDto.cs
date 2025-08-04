using ShiftTrack.Domain.Features.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Features.Booking.Common.Dtos;

public record VacationsFilterDto(
    DateTime? StartDate,
    DateTime? EndDate,
    long UnitId,
    long DepartmentId,
    VacationStatus Status,
    string SearchPattern);