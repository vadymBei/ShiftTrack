using ShiftTrack.Application.Modules.Booking.Vacations.Models;
using ShiftTrack.Application.Modules.Booking.Vacations.Constants;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Services;

/// <summary>
/// Accrues annual basic vacation by the Law of Ukraine "On Vacations":
/// <list type="bullet">
/// <item>a working year is counted from the employment start (art. 6) and lasts a calendar year of counted service;</item>
/// <item>days not counted into service (art. 9, 26) move the end of the working year;</item>
/// <item>first working year: proportional to the counted service until 6 months, then full (art. 10);</item>
/// <item>second and next years: full vacation from the first day of the year (art. 10);</item>
/// <item>unused days are carried over (no expiry in the law).</item>
/// </list>
/// </summary>
public class VacationAccrualCalculator : IVacationAccrualCalculator
{
    public IReadOnlyList<WorkingYearAccrual> Calculate(
        DateTime serviceStart,
        DateTime today,
        IReadOnlyCollection<DateRangeDto> excludedPeriods)
    {
        var start = serviceStart.Date;
        var end = today.Date;

        if (end < start)
            return [];

        var excludedDays = GetExcludedDays(excludedPeriods, start, end);
        var result = new List<WorkingYearAccrual>();

        var workingYear = 1;
        var yearStart = start;
        var requiredDays = DaysInWorkingYear(yearStart);
        var countedDays = 0;

        for (var day = start; day <= end; day = day.AddDays(1))
        {
            if (!excludedDays.Contains(day))
                countedDays++;

            if (countedDays < requiredDays)
                continue;

            result.Add(new WorkingYearAccrual(
                workingYear, yearStart, day, VacationLaw.AnnualBasicVacationDays, true, false));

            workingYear++;
            yearStart = day.AddDays(1);
            requiredDays = DaysInWorkingYear(yearStart);
            countedDays = 0;
        }

        if (yearStart <= end)
        {
            var (days, isFull) = workingYear == 1
                ? FirstYearEntitlement(yearStart, countedDays, requiredDays)
                : (VacationLaw.AnnualBasicVacationDays, true);

            result.Add(new WorkingYearAccrual(
                workingYear,
                yearStart,
                end.AddDays(requiredDays - countedDays),
                days,
                isFull,
                true));
        }

        return result;
    }

    public DateTime GetAnnualVacationAvailableFrom(
        DateTime serviceStart,
        IReadOnlyCollection<DateRangeDto> excludedPeriods)
    {
        var start = serviceStart.Date;
        var requiredDays = FullEntitlementDays(start);

        var lastExcludedDay = excludedPeriods.Count > 0
            ? excludedPeriods.Max(x => x.End.Date)
            : start;

        var excludedDays = GetExcludedDays(excludedPeriods, start,
            lastExcludedDay > start ? lastExcludedDay : start);

        var countedDays = 0;
        var day = start;

        while (true)
        {
            if (!excludedDays.Contains(day))
                countedDays++;

            if (countedDays >= requiredDays)
                return day.AddDays(1);

            day = day.AddDays(1);
        }
    }

    private static int FullEntitlementDays(DateTime yearStart) =>
        (yearStart.AddMonths(VacationLaw.FirstYearFullVacationAfterMonths) - yearStart).Days;

    private static (decimal Days, bool IsFull) FirstYearEntitlement(DateTime yearStart, int countedDays,
        int requiredDays)
    {
        var fullAfterDays = FullEntitlementDays(yearStart);

        if (countedDays >= fullAfterDays)
            return (VacationLaw.AnnualBasicVacationDays, true);

        var proportional = (decimal)VacationLaw.AnnualBasicVacationDays * countedDays / requiredDays;

        return (Math.Round(proportional, 2, MidpointRounding.AwayFromZero), false);
    }

    private static int DaysInWorkingYear(DateTime yearStart) => (yearStart.AddYears(1) - yearStart).Days;

    private static HashSet<DateTime> GetExcludedDays(IReadOnlyCollection<DateRangeDto> periods, DateTime start,
        DateTime end)
    {
        var days = new HashSet<DateTime>();

        foreach (var period in periods)
        {
            var from = period.Start.Date < start ? start : period.Start.Date;
            var to = period.End.Date > end ? end : period.End.Date;

            for (var day = from; day <= to; day = day.AddDays(1))
                days.Add(day);
        }

        return days;
    }
}
