using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShiftTrack.Domain.Features.System.User.EmployeeRoles.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum RoleScope
{
    None,
    Local,
    Global
}