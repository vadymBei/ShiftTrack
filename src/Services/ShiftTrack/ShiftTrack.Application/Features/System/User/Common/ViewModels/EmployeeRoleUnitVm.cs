using AutoMapper;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Enums;

namespace ShiftTrack.Application.Features.System.User.Common.ViewModels;

[AutoMap(typeof(EmployeeRoleUnit))]
public class EmployeeRoleUnitVm
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }
    public UnitVm Unit { get; set; }
    public List<EmployeeRoleUnitDepartmentVm> Departments { get; set; }
}