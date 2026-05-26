using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Strategies;

public class VacationStrategyFactory(
    IEmployeeRoleChecker employeeRoleChecker,
    IEnumerable<IVacationStrategy> strategies) : IVacationStrategyFactory
{
    public IVacationStrategy GetStrategy()
    {
        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
            return strategies.OfType<SysAdminVacationStrategy>().First();

        if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
            return strategies.OfType<UnitDirectorVacationStrategy>().First();

        if (employeeRoleChecker.HasCurrentUserDepartmentDirectorRole())
            return strategies.OfType<DepartmentDirectorVacationStrategy>().First();
        
        return strategies.OfType<DefaultVacationStrategy>().First();
    }
}