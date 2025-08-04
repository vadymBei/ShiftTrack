using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShiftTrack.Domain.Features.System.User.Employees.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum EmployeeGender
{
    None,
    Male,
    Female
}