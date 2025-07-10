using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Features.System.Auth.Models;

public class CurrentUser
{
    public Employee Employee { get; set; }
    public List<string> Roles { get; set; } 
}