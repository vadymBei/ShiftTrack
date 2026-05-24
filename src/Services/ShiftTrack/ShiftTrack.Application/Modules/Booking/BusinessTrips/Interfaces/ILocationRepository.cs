using ShiftTrack.Application.Modules.Booking.BusinessTrips.Models;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;

public interface ILocationRepository
{
    Task<IEnumerable<Location>> GetListByIds(IEnumerable<string> ids, CancellationToken cancellationToken);
}