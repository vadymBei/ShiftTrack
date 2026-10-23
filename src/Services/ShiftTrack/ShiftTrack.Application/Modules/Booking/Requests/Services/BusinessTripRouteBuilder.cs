using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

namespace ShiftTrack.Application.Modules.Booking.Requests.Services;

public static class BusinessTripRouteBuilder
{
    public static async Task<IDictionary<long, string>> BuildRoutes(
        IReadOnlyCollection<BusinessTrip> businessTrips,
        ILocationRepository locationRepository,
        CancellationToken cancellationToken)
    {
        if (businessTrips.Count == 0)
            return new Dictionary<long, string>();

        var integrationIds = businessTrips
            .SelectMany(x => x.Locations ?? [])
            .Select(x => x.LocationIntegrationId)
            .ToHashSet();

        var locations = await locationRepository.GetListByIds(integrationIds, cancellationToken);

        var names = locations
            .GroupBy(x => x.IntegrationId)
            .ToDictionary(x => x.Key, x => x.First().Name);

        return businessTrips.ToDictionary(
            x => x.Id,
            x => BuildRoute(x, names));
    }

    private static string BuildRoute(BusinessTrip businessTrip, IDictionary<string, string> names)
    {
        var stops = (businessTrip.Locations ?? [])
            .OrderBy(x => x.Id)
            .Select(x => names.TryGetValue(x.LocationIntegrationId, out var name) ? name : null)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        return stops.Count switch
        {
            0 => null,
            1 => stops[0],
            _ => $"{stops[0]} \u2192 {stops[^1]}"
        };
    }
}
