using ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.Interfaces;

public interface IPositionRepository
{
    Task<Position> Create(PositionToCreateDto positionToCreateDto, CancellationToken cancellationToken);
    Task<Position> Update(PositionToUpdateDto positionToUpdateDto, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Position>> GetAll(CancellationToken cancellationToken);
    Task<Position> GetById(long id, CancellationToken cancellationToken);
}