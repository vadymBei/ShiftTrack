using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.Outbox.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Constants;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Events;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Domain.Modules.Booking.Vacations.Models;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Services;

public class VacationService(
    ICurrentUserService currentUserService,
    IVacationRepository vacationRepository,
    IVacationStrategyFactory vacationStrategyFactory,
    IVacationPermissionService vacationPermissionService,
    IVacationAccrualService vacationAccrualService,
    IDomainEventOutbox domainEventOutbox) : IVacationService
{
    private IVacationStrategy VacationStrategy => vacationStrategyFactory.GetStrategy();

    public Task<Vacation> GetById(long id, CancellationToken cancellationToken)
    {
        return VacationStrategy.GetVacationById(id, cancellationToken);
    }

    public async Task<Vacation> Approve(long id, CancellationToken cancellationToken)
    {
        var current = await vacationRepository.GetById(id, cancellationToken);
        var oldStatus = current.Status;

        await vacationAccrualService.EnsureCanTakeVacation(
            current.EmployeeId, current.Type, current.StartDate, cancellationToken);

        await VacationStrategy.ApproveVacation(id, cancellationToken);

        var vacation = await VacationStrategy.GetVacationById(id, cancellationToken);

        await PublishStatusChanged(vacation, oldStatus, cancellationToken);

        return vacation;
    }

    public async Task<IEnumerable<Vacation>> GetFiltered(VacationsFilterDto filter, CancellationToken cancellationToken)
    {
        var access = VacationStrategy.GetAccessFilter();

        if (access is null)
            return [];

        return await vacationRepository.GetFiltered(filter, access, cancellationToken);
    }

    public async Task<Vacation> Reject(long id, CancellationToken cancellationToken)
    {
        var oldStatus = (await vacationRepository.GetById(id, cancellationToken)).Status;

        await VacationStrategy.RejectVacation(id, cancellationToken);

        var vacation = await VacationStrategy.GetVacationById(id, cancellationToken);

        await PublishStatusChanged(vacation, oldStatus, cancellationToken);

        return vacation;
    }

    private async Task PublishStatusChanged(Vacation vacation, VacationStatus oldStatus,
        CancellationToken cancellationToken)
    {
        if (vacation.Status == oldStatus)
            return;

        await domainEventOutbox.Enqueue(
            new VacationStatusChangedEvent(vacation.Id, oldStatus, vacation.Status),
            cancellationToken);
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        if (!vacationPermissionService.CanDelete(vacation))
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_DELETE_OTHERS_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_DELETE_OTHERS_VACATION));
        }

        await vacationRepository.Delete(vacation.Id, cancellationToken);

        await domainEventOutbox.Enqueue(
            new VacationDeletedEvent(
                vacation.Id,
                vacation.EmployeeId,
                vacation.AuthorId,
                vacation.Type,
                vacation.StartDate,
                vacation.EndDate),
            cancellationToken);
    }

    public Task<IEnumerable<Vacation>> GetMy(CancellationToken cancellationToken)
    {
        return vacationRepository.GetByEmployeeOrAuthor(currentUserService.Employee.Id, cancellationToken);
    }

    public async Task<IEnumerable<Vacation>> GetPendingApprovals(int take, CancellationToken cancellationToken)
    {
        var filter = VacationStrategy.GetApprovalFilter();

        if (filter is null)
            return [];

        return await vacationRepository.GetForApproval(filter, take, cancellationToken);
    }

    public async Task<int> CountPendingApprovals(CancellationToken cancellationToken)
    {
        var filter = VacationStrategy.GetApprovalFilter();

        if (filter is null)
            return 0;

        return await vacationRepository.CountForApproval(filter, cancellationToken);
    }

    public Task<VacationBalance> GetVacationBalanceByEmployeeId(long employeeId,
        CancellationToken cancellationToken)
    {
        return vacationAccrualService.GetBalance(employeeId, cancellationToken);
    }
}