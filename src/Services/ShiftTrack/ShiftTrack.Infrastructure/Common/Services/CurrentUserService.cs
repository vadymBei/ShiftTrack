using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Infrastructure.Common.Services;

public class CurrentUserService : ICurrentUserService
{
    public Employee Employee { get; set; }
    public List<string> Roles => Employee.EmployeeRoles.Select(x => x.Role.Name).ToList();
}