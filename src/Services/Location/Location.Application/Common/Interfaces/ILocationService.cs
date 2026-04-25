using Location.Domain.Entities;

namespace Location.Application.Common.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<LocationEntity>> Search(string query, CancellationToken cancellationToken);
}