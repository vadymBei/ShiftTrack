namespace ShiftTrack.Core.Application.System.User.Common.Interfaces;

public interface IEmployeeRoleChecker
{
    bool HasCurrentUserRole(string roleName);
}