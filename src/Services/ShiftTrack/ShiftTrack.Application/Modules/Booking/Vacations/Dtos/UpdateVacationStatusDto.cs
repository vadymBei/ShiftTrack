using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Dtos;

public record UpdateVacationStatusDto(
    long Id,
    VacationStatus Status);