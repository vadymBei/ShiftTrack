using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Employee Employee { get; set; }
    List<string> Roles { get; }
}