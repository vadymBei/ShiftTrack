namespace ShiftTrack.Application.Modules.System.User.Common.Interfaces;

public interface IEmployeeRoleChecker
{
    bool HasCurrentUserSysAdminRole();
    bool HasCurrentUserUnitDirectorRole();
    bool HasCurrentUserDepartmentDirectorRole();
}