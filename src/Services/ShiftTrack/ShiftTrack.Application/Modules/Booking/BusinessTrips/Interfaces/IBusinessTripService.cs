using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;

public interface IBusinessTripService
{
    Task <BusinessTrip> Create(BusinessTripToCreateDto dto, CancellationToken cancellationToken);
}