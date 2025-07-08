using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShiftTrack.Core.Domain.Booking.Vacations.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum VacationType
{
    None,
    YearMainVacation,
    VacationWithoutSalaryByFamily,
    VacationWithoutSalaryByMilitary,
    VacationWithoutSalaryByMilitaryOutCountry,
    VacationWithoutSalaryByPregnancy,
    VacationWithoutSalaryByChild3Years,
    VacationWithoutSalaryByChild6Years,
    BonusVacation
}