using AutoMapper;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Enums;

namespace ShiftTrack.Application.Features.System.User.Common.ViewModels;

[AutoMap(typeof(EmployeeRole))]
public class EmployeeRoleVm
{
    public long Id { get; set; }
    public RoleScope Scope { get; set; }
    public RoleVm Role { get; set; }
    public List<EmployeeRoleUnitVm> Units { get; set; }
}