using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Enums;
using ShiftTrack.Data.Interfaces;

namespace ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;

public class EmployeeRoleUnit : IAuditable
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }

    public long? EmployeeRoleId { get; set; }
    public EmployeeRole EmployeeRole { get; set; }
    
    public long? UnitId { get; set; }
    public Unit Unit { get; set; }

    public List<EmployeeRoleUnitDepartment> Departments { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public long? CreatedById { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedById { get; set; }
}