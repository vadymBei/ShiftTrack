using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Structure.Common.Services;

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