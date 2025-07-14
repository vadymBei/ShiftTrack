using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.Constants;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;

namespace ShiftTrack.Application.Features.System.User.Common.Services;

public sealed class EmployeeRoleChecker(
    ICurrentUserService currentUserService) : IEmployeeRoleChecker
{
    public bool HasCurrentUserSysAdminRole()
    {
        return HasCurrentUserRole(DefaultRolesCatalog.SYS_ADMIN);
    }

    public bool HasCurrentUserUnitDirectorRole()
    {
        return HasCurrentUserRole(DefaultRolesCatalog.UNIT_DIRECTOR);
    }
    
    private bool HasCurrentUserRole(string roleName)
    {
        return currentUserService.Employee.EmployeeRoles.Any(x => x.Role.Name == roleName);
    }
}