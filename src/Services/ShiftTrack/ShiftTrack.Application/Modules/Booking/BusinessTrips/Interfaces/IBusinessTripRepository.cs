using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;

public interface IBusinessTripRepository
{
    Task<BusinessTrip> Create(BusinessTrip businessTrip, CancellationToken cancellationToken);
    Task<IEnumerable<BusinessTrip>> GetFiltered(BusinessTripFilterDto filter, CancellationToken cancellationToken);
    Task<BusinessTrip> GetById(long id, CancellationToken cancellationToken);
    Task Update(BusinessTrip businessTrip, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
}