using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Constants;

/// <summary>
/// Rules of the Law of Ukraine "On Vacations" (No. 504/96-VR) used to accrue annual vacation.
/// </summary>
public static class VacationLaw
{
    /// <summary>Art. 6: annual basic vacation is at least 24 calendar days per worked working year.</summary>
    public const int AnnualBasicVacationDays = 24;

    /// <summary>Art. 10: full annual vacation in the first working year after 6 months of continuous work.</summary>
    public const int FirstYearFullVacationAfterMonths = 6;

    /// <summary>Vacation types that are taken from the annual basic vacation balance.</summary>
    public static readonly IReadOnlyCollection<VacationType> AnnualVacationTypes =
    [
        VacationType.YearMainVacation
    ];

    /// <summary>
    /// Periods that are not counted into the service giving the right to annual vacation, so they shift
    /// the end of the working year:
    /// art. 26 (unpaid leave by agreement, since Law No. 3494-IX of 22.11.2023),
    /// art. 9 p. 3 (child care leave until 3 years) and art. 9 p. 4 (unpaid child care leave until 6 years).
    /// Maternity leave (art. 17) is counted.
    /// </summary>
    public static readonly IReadOnlyCollection<VacationType> NotCountedInServiceTypes =
    [
        VacationType.VacationWithoutSalaryByFamily,
        VacationType.VacationWithoutSalaryByChild3Years,
        VacationType.VacationWithoutSalaryByChild6Years
    ];
}
