using ShiftTrack.Application.Modules.Booking.BusinessTrips.Models;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.DownloadBusinessTripOrderPdf;

public class BusinessTripOrderData
{
    public BusinessTrip BusinessTrip { get; set; }
    public IEnumerable<Location> Locations { get; set; }
}