using AutoMapper;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Enums;

namespace ShiftTrack.Core.Application.System.User.Common.ViewModels;

[AutoMap(typeof(EmployeeRole))]
public class EmployeeRoleVm
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }
    public RoleVM Role { get; set; }
    public List<EmployeeRoleUnitVm> Units { get; set; }
}