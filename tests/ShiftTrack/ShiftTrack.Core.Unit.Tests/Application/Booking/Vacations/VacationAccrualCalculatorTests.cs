using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Services;

namespace ShiftTrack.Core.Unit.Tests.Application.Booking.Vacations;

/// <summary>Accrual by the Law of Ukraine "On Vacations" (art. 6, 9, 10, 26).</summary>
public class VacationAccrualCalculatorTests
{
    private readonly VacationAccrualCalculator _calculator = new();

    private static DateRangeDto Excluded(DateTime start, DateTime end) => new(start, end);

    [Fact]
    public void FirstYear_BeforeSixMonths_IsProportional()
    {
        // 90 days of 365: 24 * 90 / 365 = 5.92
        var years = _calculator.Calculate(new DateTime(2026, 1, 1), new DateTime(2026, 3, 31), []);

        var year = Assert.Single(years);
        Assert.Equal(1, year.WorkingYear);
        Assert.Equal(5.92m, year.Days);
        Assert.False(year.IsFullEntitlement);
        Assert.True(year.IsCurrent);
        Assert.Equal(new DateTime(2026, 12, 31), year.PeriodEnd);
    }

    [Fact]
    public void FirstYear_DayBeforeSixMonths_IsStillProportional()
    {
        var year = Assert.Single(_calculator.Calculate(new DateTime(2026, 1, 1), new DateTime(2026, 6, 29), []));

        Assert.Equal(11.84m, year.Days);
        Assert.False(year.IsFullEntitlement);
    }

    [Fact]
    public void FirstYear_AfterSixMonths_IsFull()
    {
        var year = Assert.Single(_calculator.Calculate(new DateTime(2026, 1, 1), new DateTime(2026, 6, 30), []));

        Assert.Equal(24m, year.Days);
        Assert.True(year.IsFullEntitlement);
    }

    [Fact]
    public void NextYears_AreFullFromTheFirstDay_AndUnusedDaysCarryOver()
    {
        var years = _calculator.Calculate(new DateTime(2024, 10, 25), new DateTime(2026, 10, 25), []);

        Assert.Equal(3, years.Count);
        Assert.Equal(new DateTime(2025, 10, 24), years[0].PeriodEnd);
        Assert.Equal(new DateTime(2026, 10, 24), years[1].PeriodEnd);
        Assert.Equal(new DateTime(2026, 10, 25), years[2].PeriodStart);
        Assert.True(years[2].IsCurrent);
        Assert.All(years, year => Assert.Equal(24m, year.Days));
        Assert.Equal(72m, years.Sum(x => x.Days));
    }

    [Fact]
    public void NotCountedLeave_MovesTheEndOfTheWorkingYear()
    {
        var unpaidLeave = Excluded(new DateTime(2025, 3, 1), new DateTime(2025, 3, 10));

        var current = Assert.Single(
            _calculator.Calculate(new DateTime(2025, 1, 1), new DateTime(2026, 1, 5), [unpaidLeave]));
        Assert.Equal(new DateTime(2026, 1, 10), current.PeriodEnd);

        var years = _calculator.Calculate(new DateTime(2025, 1, 1), new DateTime(2026, 1, 12), [unpaidLeave]);
        Assert.Equal(2, years.Count);
        Assert.Equal(new DateTime(2026, 1, 10), years[0].PeriodEnd);
        Assert.Equal(new DateTime(2026, 1, 11), years[1].PeriodStart);
    }

    [Fact]
    public void FirstDay_TimeOfRegistrationIsIgnored()
    {
        var year = Assert.Single(
            _calculator.Calculate(new DateTime(2026, 10, 25, 13, 0, 0), new DateTime(2026, 10, 25), []));

        Assert.Equal(0.07m, year.Days);
    }

    [Fact]
    public void StartInTheFuture_HasNoWorkingYears()
    {
        Assert.Empty(_calculator.Calculate(new DateTime(2026, 10, 26), new DateTime(2026, 10, 25), []));
    }

    [Fact]
    public void AnnualVacationAvailableFrom_IsTheDayAfterSixMonths()
    {
        Assert.Equal(new DateTime(2026, 7, 1),
            _calculator.GetAnnualVacationAvailableFrom(new DateTime(2026, 1, 1), []));

        Assert.Equal(new DateTime(2027, 4, 21),
            _calculator.GetAnnualVacationAvailableFrom(new DateTime(2026, 10, 21, 14, 0, 0), []));
    }

    [Fact]
    public void AnnualVacationAvailableFrom_IsMovedByNotCountedLeave()
    {
        var unpaidLeave = Excluded(new DateTime(2026, 3, 1), new DateTime(2026, 3, 10));

        Assert.Equal(new DateTime(2026, 7, 11),
            _calculator.GetAnnualVacationAvailableFrom(new DateTime(2026, 1, 1), [unpaidLeave]));
    }
}
