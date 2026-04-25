using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Common.Dtos;
using ShiftTrack.Application.Modules.Booking.Common.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Common.Strategies;

public class VacationSysAdminStrategy(
    IApplicationDbContext applicationDbContext,
    ICommonVacationService commonVacationService) : IVacationStrategy
{
    public async Task ApproveVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await commonVacationService.GetVacationForStatusChange(id, cancellationToken);

        vacation.Status = VacationStatus.ApprovedByUnitDirector;

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        await commonVacationService.SetVacationShifts(id, cancellationToken);
    }

    public async Task RejectVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await commonVacationService.GetVacationForStatusChange(id, cancellationToken);
        
        vacation.Status = VacationStatus.Rejected;

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        await commonVacationService.RestoreEmployeeShiftsBeforeVacation(id, cancellationToken);
    }

    public async Task<Vacation> GetVacationById(long id, CancellationToken cancellationToken)
    {
        var vacation = await commonVacationService.GetVacationById(id, cancellationToken);

        return vacation;
    }

    public async Task<IEnumerable<Vacation>> GetVacations(VacationsFilterDto filter,
        CancellationToken cancellationToken)
    {
        var vacationsQuery = commonVacationService.GetVacationsQuery(filter);

        var vacations = await vacationsQuery
            .ToListAsync(cancellationToken);

        return vacations;
    }
}