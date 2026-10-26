namespace ShiftTrack.Application.Modules.Booking.Vacations.Models;

public record WorkingYearAccrual(
    int WorkingYear,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    decimal Days,
    bool IsFullEntitlement,
    bool IsCurrent);
