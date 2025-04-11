using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;

public class EmployeeRoleUnitDepartment
{
    public long Id { get; set; }
    
    public long? EmployeeRoleUnitId { get; set; }
    public EmployeeRoleUnit EmployeeRoleUnit { get; set; }
    
    public long? DepartmentId { get; set; }
    public Department Department { get; set; }
}