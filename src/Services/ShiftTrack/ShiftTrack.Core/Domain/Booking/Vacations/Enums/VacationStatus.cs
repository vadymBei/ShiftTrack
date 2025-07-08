using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShiftTrack.Core.Domain.Booking.Vacations.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum VacationStatus
{
    None,
    Approved,
    Rejected
}