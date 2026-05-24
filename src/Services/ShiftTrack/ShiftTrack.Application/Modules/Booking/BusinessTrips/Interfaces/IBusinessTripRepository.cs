using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;

public interface IBusinessTripRepository
{
    Task<BusinessTrip> Create(BusinessTrip businessTrip, CancellationToken cancellationToken);
}