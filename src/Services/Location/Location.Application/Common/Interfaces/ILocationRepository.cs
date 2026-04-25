using Location.Domain.Entities;

namespace Location.Application.Common.Interfaces;

public interface ILocationRepository
{
    Task<IEnumerable<LocationEntity>> Search(string query, CancellationToken cancellationToken);
}