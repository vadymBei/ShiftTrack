namespace ShiftTrack.Application.Features.System.User.Common.Interfaces;

public interface IEmployeeRoleChecker
{
    bool HasCurrentUserSysAdminRole();
    bool HasCurrentUserUnitDirectorRole();
    bool HasCurrentUserDepartmentDirectorRole();
}