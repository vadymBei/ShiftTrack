using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Structure.Common.Services;

public class PositionService(
    IApplicationDbContext applicationDbContext) : IPositionService
{
    public async Task<Position> GetById(object id, CancellationToken cancellationToken)
    {
        var positionId = (long)id;

        var position = await applicationDbContext.Positions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == positionId, cancellationToken);

        if (position is null)
        {
            throw new EntityNotFoundException(typeof(Position), positionId);
        }

        return position;
    }
}