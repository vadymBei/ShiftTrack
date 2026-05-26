using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.Organization.Structure.Positions.Repositories;

public class PositionRepository(
    IApplicationDbContext applicationDbContext)
    : IPositionRepository
{
    public async Task<Position> Create(PositionToCreateDto positionToCreateDto, CancellationToken cancellationToken)
    {
        var position = new Position()
        {
            Name = positionToCreateDto.Name,
            Description = positionToCreateDto.Description,
            HourlyRate = positionToCreateDto.HourlyRate
        };

        applicationDbContext.Positions.Add(position);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return position;
    }

    public async Task<Position> Update(PositionToUpdateDto positionToUpdateDto, CancellationToken cancellationToken)
    {
        var position = await applicationDbContext.Positions
                           .FindAsync(positionToUpdateDto.Id)
                       ?? throw new EntityNotFoundException(typeof(Position), positionToUpdateDto.Id);

        position.Name = positionToUpdateDto.Name;
        position.Description = positionToUpdateDto.Description;
        position.HourlyRate = positionToUpdateDto.HourlyRate;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return position;
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var position = await applicationDbContext.Positions
                           .FindAsync([id], cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Position), id);

        applicationDbContext.Positions.Remove(position);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Position>> GetAll(CancellationToken cancellationToken)
    {
        return await applicationDbContext.Positions
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Position> GetById(long id, CancellationToken cancellationToken)
    {
        var position = await applicationDbContext.Positions
                           .AsNoTracking()
                           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Position), id);

        return position;
    }
}