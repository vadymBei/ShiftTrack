namespace ShiftTrack.Application.Features.System.User.Common.Interfaces;

public interface IEmployeeRoleChecker
{
    bool HasCurrentUserRole(string roleName);
}