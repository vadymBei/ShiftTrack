using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Common.Services;

public class ShiftService(
    IApplicationDbContext applicationDbContext) : IShiftService
{
    public async Task<Shift> GetById(object id, CancellationToken cancellationToken)
    {
        var shiftId = (long)id;

        var shift = await applicationDbContext.Shifts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == shiftId, cancellationToken);

        if (shift == null)
        {
            throw new EntityNotFoundException(typeof(Shift), shiftId);
        }

        return shift;
    }
}