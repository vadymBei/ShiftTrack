using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.Services;

public class ShiftService(
    IApplicationDbContext applicationDbContext) : IShiftService
{
    public async Task<Shift> GetById(object id, CancellationToken cancellationToken)
    {
        var shiftId = (long)id;

        var shift = await applicationDbContext.Shifts
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == shiftId, cancellationToken)
                    ?? throw new EntityNotFoundException(typeof(Shift), shiftId);

        return shift;
    }

    public async Task<IEnumerable<Shift>> GetShiftsByIds(IEnumerable<long> shiftIds,
        CancellationToken cancellationToken)
    {
        var shifts = await applicationDbContext.Shifts
            .AsNoTracking()
            .Where(x => shiftIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return shifts;
    }

    public async Task<Shift> GetShiftByCode(string code, CancellationToken cancellationToken)
    {
        var shift = await applicationDbContext.Shifts
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Code == code, cancellationToken)
                    ?? throw new EntityNotFoundException($"Shift with code '{code}' not found");

        return shift;
    }
}