using ShiftTrack.Core.Domain.System.User.Employees.Entities;

namespace ShiftTrack.Core.Domain.System.User.Employees.Models;

public class CurrentUser
{
    public Employee Employee { get; set; }
    public List<string> Roles { get; set; }
}