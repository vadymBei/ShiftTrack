using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum ShiftType
{
    None,
    Workday,
    Holiday,
    Vacation,
    DayOff
}