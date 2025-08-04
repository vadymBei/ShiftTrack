using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Enums;

namespace ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;

public class EmployeeRoleUnit : AuditableEntity
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }

    public long? EmployeeRoleId { get; set; }
    public EmployeeRole EmployeeRole { get; set; }
    
    public long? UnitId { get; set; }
    public Unit Unit { get; set; }

    public List<EmployeeRoleUnitDepartment> Departments { get; set; }
}