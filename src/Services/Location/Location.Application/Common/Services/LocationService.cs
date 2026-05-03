using Location.Application.Common.Extensions;
using Location.Application.Common.Interfaces;
using Location.Domain.Entities;

namespace Location.Application.Common.Services;

public class LocationService(
    ILocationRepository locationRepository,
    INominatimRepository nominatimRepository) : ILocationService
{
    public async Task<IEnumerable<LocationEntity>> Search(string query, CancellationToken cancellationToken)
    {
        var locations = await locationRepository.Search(query, cancellationToken);

        if (!locations.Any())
        {
            var geoLocations = await nominatimRepository.Search(query, cancellationToken);

            locations = geoLocations.Select(x => x.ToLocationEntity())
                .GroupBy(x => x.IntegrationId)
                .Select(g => g.First())
                .ToList();

            await locationRepository.Create(locations, cancellationToken);
        }

        return locations;
    }
}