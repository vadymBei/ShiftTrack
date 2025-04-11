using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Enums;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Core.Domain.System.User.Roles.Entities;

namespace ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;

public sealed class EmployeeRole
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }

    public long? EmployeeId { get; set; }
    public Employee Employee { get; set; }
    
    public long? RoleId { get; set; }
    public Role Role { get; set; }

    public List<EmployeeRoleUnit> Units { get; set; }
}