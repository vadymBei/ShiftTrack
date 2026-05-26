using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

public class EmployeeRoleUnitDepartment : AuditableEntity
{
    public long Id { get; set; }
    
    public long? EmployeeRoleUnitId { get; set; }
    public EmployeeRoleUnit EmployeeRoleUnit { get; set; }
    
    public long? DepartmentId { get; set; }
    public Department Department { get; set; }
}