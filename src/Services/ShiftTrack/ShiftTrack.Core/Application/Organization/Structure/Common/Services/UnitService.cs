using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Structure.Common.Services;

public class UnitService(
    IApplicationDbContext dbContext) : IUnitService
{
    public async Task<Unit> GetById(object id, CancellationToken cancellationToken)
    {
        var unitId = (long)id;

        var unit = await dbContext.Units
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == unitId, cancellationToken);

        if (unit == null)
            throw new EntityNotFoundException(typeof(Unit), unitId);

        return unit;
    }
}