using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.Services;

public class EmployeeShiftHistoryService(
    IApplicationDbContext applicationDbContext) : IEmployeeShiftHistoryService
{
    public async Task Create(IEnumerable<EmployeeShiftHistory> histories, CancellationToken cancellationToken)
    {
        applicationDbContext.EmployeeShiftHistories.AddRange(histories);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<EmployeeShiftHistory>> GetByEmployeeShiftId(long employeeShiftId, CancellationToken cancellationToken)
    {
        var employeeShiftHistory = await applicationDbContext.EmployeeShiftHistories
            .AsNoTracking()
            .Include(x => x.Author)
            .Include(x => x.NewShift)
            .Include(x => x.PreviousShift)
            .Where(x => x.EmployeeShiftId == employeeShiftId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return employeeShiftHistory;
    }

    public async Task<IEnumerable<EmployeeShiftHistory>> GetByEmployeeShiftIds(IEnumerable<long> employeeShiftIds, CancellationToken cancellationToken)
    {
        var employeeShiftHistory = await applicationDbContext.EmployeeShiftHistories
            .AsNoTracking()
            .Where(x => employeeShiftIds.Contains(x.EmployeeShiftId))
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return employeeShiftHistory;
    }
}