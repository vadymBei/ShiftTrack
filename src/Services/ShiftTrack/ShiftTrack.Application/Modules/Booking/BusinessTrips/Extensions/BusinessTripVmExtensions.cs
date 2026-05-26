using ShiftTrack.Application.Modules.Booking.BusinessTrips.Models;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Extensions;

public static class BusinessTripVmExtensions
{
    public static void EnrichWithLocationData(
        this BusinessTripVm vm,
        IDictionary<string, Location> locationDict)
    {
        foreach (var locationVm in vm.Locations)
        {
            if (!locationDict.TryGetValue(locationVm.LocationIntegrationId, out var location))
                continue;

            locationVm.Name = location.Name;
            locationVm.Country = location.Country;
            locationVm.CountryCode = location.CountryCode;
            locationVm.DisplayName = location.DisplayName;
        }
    }
}