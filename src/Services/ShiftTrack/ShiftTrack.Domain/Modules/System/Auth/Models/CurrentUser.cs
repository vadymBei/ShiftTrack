using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Modules.System.Auth.Models;

public class CurrentUser
{
    public Employee Employee { get; set; }
    public List<string> Roles { get; set; } 
}