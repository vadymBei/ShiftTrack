using AutoMapper;
using ShiftTrack.Core.Domain.System.User.Employees.Models;

namespace ShiftTrack.Core.Application.System.User.Common.ViewModels;

[AutoMap(typeof(CurrentUser))]
public class CurrentUserVM
{
    public EmployeeVM Employee { get; set; }
    public List<string> Roles { get; set; }
}