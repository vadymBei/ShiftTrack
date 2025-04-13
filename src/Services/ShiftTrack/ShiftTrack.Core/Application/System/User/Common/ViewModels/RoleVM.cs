using AutoMapper;
using ShiftTrack.Core.Domain.System.User.Roles.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.ViewModels;

[AutoMap(typeof(Role))]
public class RoleVM
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
}