using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;

public interface IVacationAccrualCalculator
{
    /// <summary>
    /// Working years started from <paramref name="serviceStart"/> up to <paramref name="today"/> with the days
    /// accrued for each of them. Excluded periods are not counted into service and shift the working years.
    /// </summary>
    IReadOnlyList<WorkingYearAccrualDto> Calculate(
        DateTime serviceStart,
        DateTime today,
        IReadOnlyCollection<DateRangeDto> excludedPeriods);

    /// <summary>
    /// First day annual vacation can start: after 6 months of continuous work counted from
    /// <paramref name="serviceStart"/> (art. 10). Excluded periods move this date.
    /// </summary>
    DateTime GetAnnualVacationAvailableFrom(
        DateTime serviceStart,
        IReadOnlyCollection<DateRangeDto> excludedPeriods);
}
