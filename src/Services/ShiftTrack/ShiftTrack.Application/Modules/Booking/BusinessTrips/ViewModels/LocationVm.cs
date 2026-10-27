using AutoMapper;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Models;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;

[AutoMap(typeof(Location))]
public class LocationVm
{
    public string IntegrationId { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Country { get; set; }
    public string CountryCode { get; set; }
}
