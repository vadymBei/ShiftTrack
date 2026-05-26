using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;

public interface IUnitService
{
    Task<IEnumerable<Unit>> GetUnitsByRoles(CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
}