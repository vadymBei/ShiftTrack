using Location.Application.Common.Interfaces;
using Location.Domain.Entities;
using Location.Infrastructure.Extensions;
using Location.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace Location.Infrastructure.Repositories;

public class LocationRepository(
    IClient client,
    ILogger<LocationRepository> logger) : ILocationRepository
{
    public async Task<IEnumerable<LocationEntity>> Search(string query, CancellationToken cancellationToken)
    {
        try
        {
            var geoLocations = await client
                .Path("nominatim-api/search-service")
                .Headers("User-Agent", "MyLocationApp/1.0")
                .Get<IEnumerable<GeoLocation>>($"?format=json&featureType=city&addressdetails=1&accept-language=uk&q={query}", cancellationToken);

            return geoLocations.Select(x => x.ToLocationEntity());
        }
        catch (HttpClientException ex)
        {
            logger.LogError(ex, "Error occurred while searching for locations");
            throw;
        }
    }
}