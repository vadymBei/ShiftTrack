using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Enums;

namespace ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;

public class EmployeeRoleUnit
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }

    public long? EmployeeRoleId { get; set; }
    public EmployeeRole EmployeeRole { get; set; }
    
    public long? UnitId { get; set; }
    public Unit Unit { get; set; }

    public List<EmployeeRoleUnitDepartment> Departments { get; set; }
}