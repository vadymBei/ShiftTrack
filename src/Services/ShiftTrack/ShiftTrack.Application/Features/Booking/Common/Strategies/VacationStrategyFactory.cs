using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Common.Strategies;

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