using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Domain.Modules.System.Auth.Models;

namespace ShiftTrack.Application.Modules.System.Auth.Account.ViewModels;

[AutoMap(typeof(CurrentUser))]
public class CurrentUserVm
{
    public EmployeeVm Employee { get; set; }
    public List<string> Roles { get; set; }
}