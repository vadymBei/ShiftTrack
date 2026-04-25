using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.Common.ViewModels;
using ShiftTrack.Domain.Modules.System.Auth.Models;

namespace ShiftTrack.Application.Modules.System.Auth.Common.ViewModels;

[AutoMap(typeof(CurrentUser))]
public class CurrentUserVm
{
    public EmployeeVm Employee { get; set; }
    public List<string> Roles { get; set; }
}