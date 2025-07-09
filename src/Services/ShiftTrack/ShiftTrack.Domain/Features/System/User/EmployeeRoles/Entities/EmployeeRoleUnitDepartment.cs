using ShiftTrack.Domain.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;

namespace ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;

public class EmployeeRoleUnitDepartment : IAuditable
{
    public long Id { get; set; }
    
    public long? EmployeeRoleUnitId { get; set; }
    public EmployeeRoleUnit EmployeeRoleUnit { get; set; }
    
    public long? DepartmentId { get; set; }
    public Department Department { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public long? CreatedById { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedById { get; set; }
}