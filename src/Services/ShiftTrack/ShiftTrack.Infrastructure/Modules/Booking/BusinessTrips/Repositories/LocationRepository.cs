using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Models;
using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.Booking.BusinessTrips.Repositories;

public class LocationRepository(
    IClient client,
    ILogger<LocationRepository> logger) : ILocationRepository
{
    public async Task<IEnumerable<Location>> GetListByIds(IEnumerable<string> ids, CancellationToken cancellationToken)
    {
        try
        {
            var locations = await client
                .Path("locations-api/locations-service")
                .Auth(AuthProvider.Basic)
                .Query<string>("IntegrationIds", ids)
                .Get<IEnumerable<Location>>("by-integrationIds", cancellationToken);
            
            return locations;
        }
        catch (HttpClientException e)
        {
            logger.LogError(e, "Error fetching locations from API");
            throw;
        }
    }
}