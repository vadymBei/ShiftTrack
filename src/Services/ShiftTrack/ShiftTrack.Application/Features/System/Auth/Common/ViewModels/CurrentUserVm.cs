using AutoMapper;
using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Domain.Features.System.Auth.Models;

namespace ShiftTrack.Application.Features.System.Auth.Common.ViewModels;

[AutoMap(typeof(CurrentUser))]
public class CurrentUserVm
{
    public EmployeeVm Employee { get; set; }
    public List<string> Roles { get; set; }
}