using ShiftTrack.Application.Features.System.User.Common.Constants;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;

namespace ShiftTrack.Application.Features.System.User.Common.Strategies;

public sealed class EmployeeRoleStrategyFactory(
    IEmployeeRoleChecker employeeRoleChecker,
    IEnumerable<IEmployeeRoleStrategy> strategies) : IEmployeeRoleStrategyFactory
{
    public IEmployeeRoleStrategy GetStrategy()
    {
        if (employeeRoleChecker.HasCurrentUserRole(DefaultRolesCatalog.SYS_ADMIN))
            return strategies.OfType<EmployeeRoleSysAdminStrategy>().First();

        if (employeeRoleChecker.HasCurrentUserRole(DefaultRolesCatalog.UNIT_DIRECTOR))
            return strategies.OfType<EmployeeRoleUnitDirectorStrategy>().First();
        
        return strategies.OfType<EmployeeRoleDepartmentDirectorStrategy>().First();
    }
}