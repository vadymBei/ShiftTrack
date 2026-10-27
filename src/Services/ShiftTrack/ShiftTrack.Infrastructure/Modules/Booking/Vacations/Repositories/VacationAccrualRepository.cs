using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Modules.Booking.Vacations.Repositories;

public class VacationAccrualRepository(
    IApplicationDbContext applicationDbContext) : IVacationAccrualRepository
{
    public async Task<IReadOnlyList<VacationAccrual>> GetByEmployeeId(long employeeId,
        CancellationToken cancellationToken)
    {
        return await applicationDbContext.VacationAccruals
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .OrderBy(x => x.WorkingYear)
            .ToListAsync(cancellationToken);
    }

    public async Task ReplaceForEmployees(IReadOnlyDictionary<long, IReadOnlyList<VacationAccrual>> accruals,
        CancellationToken cancellationToken)
    {
        var employeeIds = accruals.Keys.ToList();

        var existing = (await applicationDbContext.VacationAccruals
                .Where(x => employeeIds.Contains(x.EmployeeId))
                .ToListAsync(cancellationToken))
            .ToDictionary(x => (x.EmployeeId, x.WorkingYear));

        foreach (var accrual in accruals.Values.SelectMany(x => x))
        {
            if (!existing.Remove((accrual.EmployeeId, accrual.WorkingYear), out var current))
            {
                await applicationDbContext.VacationAccruals.AddAsync(accrual, cancellationToken);
                continue;
            }

            // Unchanged values aren't marked as modified, so the daily run updates only what changed.
            current.PeriodStart = accrual.PeriodStart;
            current.PeriodEnd = accrual.PeriodEnd;
            current.Days = accrual.Days;
            current.IsFullEntitlement = accrual.IsFullEntitlement;
        }

        // Working years that no longer exist (e.g. a not counted leave was approved later).
        applicationDbContext.VacationAccruals.RemoveRange(existing.Values);

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
