namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;

public interface IEmployeeRoleChecker
{
    bool HasCurrentUserSysAdminRole();
    bool HasCurrentUserUnitDirectorRole();
    bool HasCurrentUserDepartmentDirectorRole();
}