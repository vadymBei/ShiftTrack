using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Modules.Organization.Timesheet.EmployeeShifts.Repositories;

public class EmployeeShiftRepository(
    IApplicationDbContext applicationDbContext) : IEmployeeShiftRepository
{
    public async Task Create(IEnumerable<EmployeeShift> employeeShifts, CancellationToken cancellationToken)
    {
        applicationDbContext.EmployeeShifts.AddRange(employeeShifts);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(IEnumerable<EmployeeShift> employeeShifts, CancellationToken cancellationToken)
    {
        applicationDbContext.EmployeeShifts.UpdateRange(employeeShifts);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<EmployeeShift>> GetEmployeeShifts(EmployeeShiftsFilterDto dto,
        CancellationToken cancellationToken)
    {
        var query = applicationDbContext.EmployeeShifts
            .Include(x => x.Shift)
            .Where(x => dto.EmployeeIds.Contains(x.EmployeeId)
                        && x.Date >= dto.StartDate
                        && x.Date <= dto.EndDate);

        if (dto is { DisableTracking: true })
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync(cancellationToken);
    }
}