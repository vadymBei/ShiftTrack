using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Constants;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Strategies;

public class DefaultVacationStrategy(
    IVacationRepository vacationRepository,
    ICurrentUserService currentUserService) : IVacationStrategy
{
    public async Task ApproveVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        if (vacation.AuthorId != currentUserService.Employee.Id)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_VIEW_OTHERS_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_VIEW_OTHERS_VACATION));
        }
        
        if (vacation.Status is not VacationStatus.None)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_APPROVE_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_APPROVE_VACATION));
        }
        
        await vacationRepository.UpdateVacationStatus(
            new UpdateVacationStatusDto(
                vacation.Id,
                VacationStatus.PendingApproval),
            cancellationToken);
    }

    public Task RejectVacation(long id, CancellationToken cancellationToken)
    {
        throw new VacationException(VacationExceptionsLocalization.CANNOT_REJECT_VACATION,
            nameof(VacationExceptionsLocalization.CANNOT_REJECT_VACATION));
    }

    public async Task<Vacation> GetVacationById(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        if (vacation.AuthorId != currentUserService.Employee.Id)
        {
            throw new VacationException(VacationExceptionsLocalization.CANNOT_VIEW_OTHERS_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_VIEW_OTHERS_VACATION));
        }

        return vacation;
    }

    public async Task<IEnumerable<Vacation>> GetVacations(
        VacationsFilterDto filter,
        CancellationToken cancellationToken)
    {
        var vacations = await vacationRepository.GetFiltered(filter, cancellationToken);

        vacations = vacations
            .Where(x => x.AuthorId == currentUserService.Employee.Id
                        || x.Author.DepartmentId == currentUserService.Employee.DepartmentId)
            .ToList();
        
        return vacations;
    }
}