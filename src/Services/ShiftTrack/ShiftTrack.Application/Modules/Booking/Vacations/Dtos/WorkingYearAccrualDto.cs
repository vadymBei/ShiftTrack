namespace ShiftTrack.Application.Modules.Booking.Vacations.Dtos;

public record WorkingYearAccrualDto(
    int WorkingYear,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    decimal Days,
    bool IsFullEntitlement,
    bool IsCurrent);
