using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Strategies;

public sealed class EmployeeRoleStrategyFactory(
    IEmployeeRoleChecker employeeRoleChecker,
    IEnumerable<IEmployeeRoleStrategy> strategies) : IEmployeeRoleStrategyFactory
{
    public IEmployeeRoleStrategy GetStrategy()
    {
        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
            return strategies.OfType<SysAdminEmployeeRoleStrategy>().First();

        if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
            return strategies.OfType<UnitDirectorEmployeeRoleStrategy>().First();

        return strategies.OfType<DepartmentDirectorEmployeeRoleStrategy>().First();
    }
}