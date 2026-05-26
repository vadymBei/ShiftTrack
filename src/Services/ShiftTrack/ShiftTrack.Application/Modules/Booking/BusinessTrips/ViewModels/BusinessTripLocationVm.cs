using AutoMapper;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;

[AutoMap(typeof(BusinessTripLocation))]
public class BusinessTripLocationVm
{
    public string LocationIntegrationId { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Country { get; set; }
    public string CountryCode { get; set; }
}