using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.Constants;
using ShiftTrack.Application.Features.Booking.Common.Dtos;
using ShiftTrack.Application.Features.Booking.Common.Exceptions;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Features.Booking.Common.Strategies;

public class VacationDefaultStrategy(
    ICurrentUserService currentUserService,
    IApplicationDbContext applicationDbContext,
    ICommonVacationService commonVacationService) : IVacationStrategy
{
    public async Task ApproveVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await commonVacationService.GetVacationForStatusChange(id, cancellationToken);

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
        
        vacation.Status = VacationStatus.PendingApproval;
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task RejectVacation(long id, CancellationToken cancellationToken)
    {
        throw new VacationException(VacationExceptionsLocalization.CANNOT_REJECT_VACATION,
            nameof(VacationExceptionsLocalization.CANNOT_REJECT_VACATION));
    }

    public async Task<Vacation> GetVacationById(long id, CancellationToken cancellationToken)
    {
        var vacation = await commonVacationService.GetVacationById(id, cancellationToken);

        if (vacation.AuthorId != currentUserService.Employee.Id)
        {
            throw new VacationException(VacationExceptionsLocalization.CANNOT_VIEW_OTHERS_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_VIEW_OTHERS_VACATION));
        }

        return vacation;
    }

    public async Task<IEnumerable<Vacation>> GetVacations(VacationsFilterDto filter,
        CancellationToken cancellationToken)
    {
        var vacationsQuery = commonVacationService.GetVacations(filter, cancellationToken);

        var vacations = await vacationsQuery
            .Where(x => x.AuthorId == currentUserService.Employee.Id
                        || x.Author.DepartmentId == currentUserService.Employee.DepartmentId)
            .ToListAsync(cancellationToken);

        return vacations;
    }
}