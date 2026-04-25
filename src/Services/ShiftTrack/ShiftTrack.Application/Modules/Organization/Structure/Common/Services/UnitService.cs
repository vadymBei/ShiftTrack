using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Common.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.Organization.Structure.Common.Services;

public class UnitService(
    IApplicationDbContext dbContext) : IUnitService
{
    public async Task<Unit> GetById(object id, CancellationToken cancellationToken)
    {
        var unitId = (long)id;

        var unit = await dbContext.Units
                       .AsNoTracking()
                       .FirstOrDefaultAsync(x => x.Id == unitId, cancellationToken)
                   ?? throw new EntityNotFoundException(typeof(Unit), unitId);

        return unit;
    }
}