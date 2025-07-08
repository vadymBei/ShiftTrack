using ShiftTrack.Core.Domain.System.User.Employees.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces;

public interface ICurrentUserService
{
    Employee Employee { get; set; }
    List<string> Roles { get; }
}