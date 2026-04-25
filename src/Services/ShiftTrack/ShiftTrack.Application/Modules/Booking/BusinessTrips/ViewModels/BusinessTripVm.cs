using AutoMapper;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Enums;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;

[AutoMap(typeof(BusinessTrip))]
public class BusinessTripVm
{
    public long Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Route { get; set; }
    public string Description { get; set; }
    public decimal EstimatedBudget { get; set; }
    public BusinessTripStatus Status { get; set; }

    public List<BusinessTripLocationVm> Locations { get; set; }
}