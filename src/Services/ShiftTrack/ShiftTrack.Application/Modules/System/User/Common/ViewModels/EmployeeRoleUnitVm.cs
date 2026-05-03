using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Enums;

namespace ShiftTrack.Application.Modules.System.User.Common.ViewModels;

[AutoMap(typeof(EmployeeRoleUnit))]
public class EmployeeRoleUnitVm
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }
    public UnitVm Unit { get; set; }
    public List<EmployeeRoleUnitDepartmentVm> Departments { get; set; }
}