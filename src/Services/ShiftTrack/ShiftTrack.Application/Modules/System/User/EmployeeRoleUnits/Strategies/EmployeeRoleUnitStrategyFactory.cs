using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Strategies;

public class EmployeeRoleUnitStrategyFactory(
    IEmployeeRoleChecker employeeRoleChecker,
    IEnumerable<IEmployeeRoleUnitStrategy> strategies) : IEmployeeRoleUnitStrategyFactory
{
    public IEmployeeRoleUnitStrategy GetStrategy()
    {
        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
            return strategies.OfType<SysAdminEmployeeRoleUnitStrategy>().First();

        if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
            return strategies.OfType<UnitDirectorEmployeeRoleUnitStrategy>().First();
        
        return strategies.OfType<DepartmentDirectorEmployeeRoleUnitStrategy>().First();
    }
}