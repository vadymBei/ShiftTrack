using AutoMapper;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Enums;

namespace ShiftTrack.Core.Application.System.User.Common.ViewModels;

[AutoMap(typeof(EmployeeRoleUnit))]
public class EmployeeRoleUnitVm
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }
    public UnitVM Unit { get; set; }
    public List<EmployeeRoleUnitDepartmentVm> Departments { get; set; }
}