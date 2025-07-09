using ShiftTrack.Domain.Common.Interfaces;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Enums;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Domain.Features.System.User.Roles.Entities;

namespace ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;

public sealed class EmployeeRole : IAuditable
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }

    public long? EmployeeId { get; set; }
    public Employee Employee { get; set; }
    
    public long? RoleId { get; set; }
    public Role Role { get; set; }

    public List<EmployeeRoleUnit> Units { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public long? CreatedById { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedById { get; set; }
}