using AutoMapper;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Features.System.User.Common.ViewModels;

[AutoMap(typeof(EmployeeRoleUnitDepartment))]
public class EmployeeRoleUnitDepartmentVm
{
    public long Id { get; set; }
    public DepartmentVm Department { get; set; }
}