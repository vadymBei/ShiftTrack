using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Strategies;

public class EmployeeRoleUnitDepartmentStrategyFactory(
    IEmployeeRoleChecker employeeRoleChecker,
    IEnumerable<IEmployeeRoleUnitDepartmentStrategy> strategies)
    : IEmployeeRoleUnitDepartmentStrategyFactory
{
    public IEmployeeRoleUnitDepartmentStrategy GetStrategy()
    {
        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
            return strategies.OfType<SysAdminEmployeeRoleUnitDepartmentStrategy>().First();

        if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
            return strategies.OfType<UnitDirectorEmployeeRoleUnitDepartmentStrategy>().First();

        return strategies.OfType<DepartmentDirectorEmployeeRoleUnitDepartmentStrategy>().First();
    }
}