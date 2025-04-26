using AutoMapper;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.ViewModels;

[AutoMap(typeof(EmployeeRoleUnitDepartment))]
public class EmployeeRoleUnitDepartmentVm
{
    public long Id { get; set; }
    public DepartmentVM Department { get; set; }
}