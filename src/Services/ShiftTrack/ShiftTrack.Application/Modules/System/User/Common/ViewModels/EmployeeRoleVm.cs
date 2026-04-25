using AutoMapper;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Enums;

namespace ShiftTrack.Application.Modules.System.User.Common.ViewModels;

[AutoMap(typeof(EmployeeRole))]
public class EmployeeRoleVm
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }
    public RoleVm Role { get; set; }
    public List<EmployeeRoleUnitVm> Units { get; set; }
}