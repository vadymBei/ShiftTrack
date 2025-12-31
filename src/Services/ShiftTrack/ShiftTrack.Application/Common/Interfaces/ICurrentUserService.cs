using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Employee Employee { get; set; }
    List<string> Roles { get; }
}