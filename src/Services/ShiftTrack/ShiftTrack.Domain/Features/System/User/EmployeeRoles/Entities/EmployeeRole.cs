using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Enums;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Domain.Features.System.User.Roles.Entities;

namespace ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;

public sealed class EmployeeRole : AuditableEntity
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }

    public long? EmployeeId { get; set; }
    public Employee Employee { get; set; }
    
    public long? RoleId { get; set; }
    public Role Role { get; set; }

    public List<EmployeeRoleUnit> Units { get; set; }
}