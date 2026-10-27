using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Services;

public class BusinessTripRouteService(
    ILocationRepository locationRepository,
    ILogger<BusinessTripRouteService> logger) : IBusinessTripRouteService
{
    public Task<string> GetRoute(BusinessTrip businessTrip, CancellationToken cancellationToken)
    {
        return GetRoute(GetOrderedLocationIds(businessTrip), cancellationToken);
    }

    public async Task<string> GetRoute(IReadOnlyList<string> locationIntegrationIds,
        CancellationToken cancellationToken)
    {
        if (locationIntegrationIds.Count == 0)
            return null;

        var names = await LoadLocationNames(locationIntegrationIds, cancellationToken);

        return BuildRoute(locationIntegrationIds, names);
    }

    public async Task<IDictionary<long, string>> GetRoutes(IReadOnlyCollection<BusinessTrip> businessTrips,
        CancellationToken cancellationToken)
    {
        if (businessTrips.Count == 0)
            return new Dictionary<long, string>();

        var locationIds = businessTrips.ToDictionary(x => x.Id, GetOrderedLocationIds);

        var names = await LoadLocationNames(
            locationIds.Values.SelectMany(x => x).Distinct().ToList(),
            cancellationToken);

        return locationIds.ToDictionary(x => x.Key, x => BuildRoute(x.Value, names));
    }

    private static IReadOnlyList<string> GetOrderedLocationIds(BusinessTrip businessTrip)
    {
        return (businessTrip.Locations ?? [])
            .OrderBy(x => x.Id)
            .Select(x => x.LocationIntegrationId)
            .ToList();
    }

    private async Task<IReadOnlyDictionary<string, string>> LoadLocationNames(
        IReadOnlyCollection<string> locationIntegrationIds,
        CancellationToken cancellationToken)
    {
        if (locationIntegrationIds.Count == 0)
            return new Dictionary<string, string>();

        try
        {
            var locations = await locationRepository.GetListByIds(locationIntegrationIds, cancellationToken);

            return locations
                .GroupBy(x => x.IntegrationId)
                .ToDictionary(x => x.Key, x => x.First().Name);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            logger.LogWarning(exception, "Failed to load locations for business trip routes");

            return new Dictionary<string, string>();
        }
    }

    private static string BuildRoute(IReadOnlyList<string> locationIntegrationIds,
        IReadOnlyDictionary<string, string> names)
    {
        var stops = locationIntegrationIds
            .Select(id => names.TryGetValue(id, out var name) ? name : null)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .ToList();

        return stops.Count switch
        {
            0 => null,
            1 => stops[0],
            _ => $"{stops[0]} \u2192 {stops[^1]}"
        };
    }
}
