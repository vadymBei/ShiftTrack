using ShiftTrack.Application.Modules.Booking.Common.Interfaces;
using ShiftTrack.Application.Modules.System.User.Common.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Common.Strategies;

public class VacationStrategyFactory(
    IEmployeeRoleChecker employeeRoleChecker,
    IEnumerable<IVacationStrategy> strategies) : IVacationStrategyFactory
{
    public IVacationStrategy GetStrategy()
    {
        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
            return strategies.OfType<VacationSysAdminStrategy>().First();

        if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
            return strategies.OfType<VacationUnitDirectorStrategy>().First();

        if (employeeRoleChecker.HasCurrentUserDepartmentDirectorRole())
            return strategies.OfType<VacationDepartmentDirectorStrategy>().First();
        
        return strategies.OfType<VacationDefaultStrategy>().First();
    }
}