using ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;

public interface IUnitRepository
{
    Task<Unit> GetById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Unit>> GetAll(CancellationToken cancellationToken);
    Task<IEnumerable<Unit>> GetByIds(IEnumerable<long> ids, CancellationToken cancellationToken);
    Task<Unit> Create(UnitToCreateDto unitToCreateDto, CancellationToken cancellationToken);
    Task<Unit> Update(UnitToUpdateDto unitToUpdateDto, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
}