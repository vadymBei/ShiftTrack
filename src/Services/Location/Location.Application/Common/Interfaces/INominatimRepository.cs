using Location.Application.Common.Models;

namespace Location.Application.Common.Interfaces;

public interface INominatimRepository
{
    Task<IEnumerable<GeoLocation>> Search(string query, CancellationToken cancellationToken);
}