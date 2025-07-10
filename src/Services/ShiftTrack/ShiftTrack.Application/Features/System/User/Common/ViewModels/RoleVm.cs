using AutoMapper;
using ShiftTrack.Domain.Features.System.User.Roles.Entities;

namespace ShiftTrack.Application.Features.System.User.Common.ViewModels;

[AutoMap(typeof(Role))]
public class RoleVm
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
}