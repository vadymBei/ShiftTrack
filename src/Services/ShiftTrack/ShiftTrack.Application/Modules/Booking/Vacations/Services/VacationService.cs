using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Constants;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Services;

public class VacationService(
    ICurrentUserService currentUserService,
    IVacationRepository vacationRepository,
    IEmployeeRoleChecker employeeRoleChecker,
    IVacationStrategyFactory vacationStrategyFactory) : IVacationService
{
    private IVacationStrategy VacationStrategy => vacationStrategyFactory.GetStrategy();

    public Task<Vacation> GetById(long id, CancellationToken cancellationToken)
    {
        return VacationStrategy.GetVacationById(id, cancellationToken);
    }

    public async Task<Vacation> Approve(long id, CancellationToken cancellationToken)
    {
        await VacationStrategy.ApproveVacation(id, cancellationToken);

        return await VacationStrategy.GetVacationById(id, cancellationToken);
    }

    public Task<IEnumerable<Vacation>> GetFiltered(VacationsFilterDto filter, CancellationToken cancellationToken)
    {
        return VacationStrategy.GetVacations(filter, cancellationToken);
    }

    public async Task<Vacation> Reject(long id, CancellationToken cancellationToken)
    {
        await VacationStrategy.RejectVacation(id, cancellationToken);

        return await VacationStrategy.GetVacationById(id, cancellationToken);
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        if (!employeeRoleChecker.HasCurrentUserSysAdminRole()
            && vacation.AuthorId != currentUserService.Employee.Id
            && vacation.Status == VacationStatus.PendingApproval)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_DELETE_OTHERS_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_DELETE_OTHERS_VACATION));
        }

        await vacationRepository.Delete(vacation.Id, cancellationToken);
    }
}