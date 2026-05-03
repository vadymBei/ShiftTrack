using Location.Domain.Entities;

namespace Location.Application.Common.Interfaces;

public interface ILocationRepository
{
    Task<IEnumerable<LocationEntity>> GetByIntegrationIds(IEnumerable<string> integrationIds, CancellationToken cancellationToken);
    Task<IEnumerable<LocationEntity>> Search(string query, CancellationToken cancellationToken);
    Task<IEnumerable<LocationEntity>> Create(IEnumerable<LocationEntity> entities, CancellationToken cancellationToken);
}