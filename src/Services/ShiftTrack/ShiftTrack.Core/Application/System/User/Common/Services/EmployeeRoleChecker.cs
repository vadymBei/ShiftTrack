using ShiftTrack.Core.Application.System.User.Common.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Common.Services;

public sealed class EmployeeRoleChecker(
    ICurrentUserService currentUserService) : IEmployeeRoleChecker
{
    public bool HasCurrentUserRole(string roleName)
    {
        return currentUserService.Employee.EmployeeRoles.Any(x => x.Role.Name == roleName);
    }
}