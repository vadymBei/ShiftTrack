using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.Constants;
using ShiftTrack.Application.Features.Booking.Common.Dtos;
using ShiftTrack.Application.Features.Booking.Common.Exceptions;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Features.Booking.Common.Strategies;

public class VacationDepartmentDirectorStrategy(
    ICurrentUserService currentUserService,
    IApplicationDbContext applicationDbContext,
    ICommonVacationService commonVacationService) : IVacationStrategy
{
    public async Task ApproveVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await commonVacationService.GetVacationForStatusChange(id, cancellationToken);

        ValidateVacationForStatusChange(vacation);

        vacation.Status = VacationStatus.ApprovedByDepartmentDirector;

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    private void ValidateVacationForStatusChange(Vacation vacation)
    {
        if (vacation.Employee.DepartmentId != currentUserService.Employee.DepartmentId)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_CHANGE_VACATION_STATUS_FROM_OTHER_DEPARTMENT,
                nameof(VacationExceptionsLocalization.CANNOT_CHANGE_VACATION_STATUS_FROM_OTHER_DEPARTMENT));
        }

        if (vacation.Status is VacationStatus.ApprovedByUnitDirector)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_APPROVE_UNIT_DIRECTOR_APPROVED_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_APPROVE_UNIT_DIRECTOR_APPROVED_VACATION));
        }
    }

    public async Task RejectVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await commonVacationService.GetVacationForStatusChange(id, cancellationToken);

        ValidateVacationForStatusChange(vacation);

        vacation.Status = VacationStatus.Rejected;

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Vacation> GetVacationById(long id, CancellationToken cancellationToken)
    {
        var vacation = await commonVacationService.GetVacationById(id, cancellationToken);

        if (vacation.Employee.DepartmentId != currentUserService.Employee.DepartmentId)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_VIEW_VACATION_FROM_OTHER_DEPARTMENT,
                nameof(VacationExceptionsLocalization.CANNOT_VIEW_VACATION_FROM_OTHER_DEPARTMENT));
        }

        return vacation;
    }

    public async Task<IEnumerable<Vacation>> GetVacations(VacationsFilterDto filter, CancellationToken cancellationToken)
    {
        var vacationsQuery = commonVacationService.GetVacations(filter, cancellationToken);

        var vacations = await vacationsQuery
            .Where(x => x.AuthorId == currentUserService.Employee.Id
                        || x.Author.DepartmentId == currentUserService.Employee.DepartmentId)
            .ToListAsync(cancellationToken);

        return vacations;
    }
}