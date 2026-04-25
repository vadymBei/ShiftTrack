using AutoMapper;
using ShiftTrack.Domain.Modules.System.User.Roles.Entities;

namespace ShiftTrack.Application.Modules.System.User.Common.ViewModels;

[AutoMap(typeof(Role))]
public class RoleVm
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
}