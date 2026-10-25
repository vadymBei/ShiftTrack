using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;

/// <summary>
/// Builds a short business trip route ("first stop → last stop") from location names.
/// Returns <c>null</c> when the route can't be resolved (no locations or the location service is unavailable).
/// </summary>
public interface IBusinessTripRouteService
{
    Task<string> GetRoute(BusinessTrip businessTrip, CancellationToken cancellationToken);

    Task<string> GetRoute(IReadOnlyList<string> locationIntegrationIds, CancellationToken cancellationToken);

    Task<IDictionary<long, string>> GetRoutes(IReadOnlyCollection<BusinessTrip> businessTrips,
        CancellationToken cancellationToken);
}
