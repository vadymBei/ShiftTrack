using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.Common.ViewModels;

[AutoMap(typeof(EmployeeRoleUnitDepartment))]
public class EmployeeRoleUnitDepartmentVm
{
    public long Id { get; set; }
    public DepartmentVm Department { get; set; }
}