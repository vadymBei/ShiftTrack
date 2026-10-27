using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;

public interface IBusinessTripService
{
    Task<BusinessTrip> Create(BusinessTripToCreateDto dto, CancellationToken cancellationToken);
    Task<BusinessTrip> Update(BusinessTripToUpdateDto dto, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
    Task Approve(long id, CancellationToken cancellationToken);
    Task Reject(long id, CancellationToken cancellationToken);
    Task<IEnumerable<BusinessTrip>> GetFiltered(BusinessTripFilterDto filter, CancellationToken cancellationToken);
    Task<BusinessTrip> GetById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<BusinessTrip>> GetMy(CancellationToken cancellationToken);
    Task<IEnumerable<BusinessTrip>> GetPendingApprovals(int take, CancellationToken cancellationToken);
    Task<int> CountPendingApprovals(CancellationToken cancellationToken);
    Task<BusinessTripPermissionsVm> GetPermissions(BusinessTrip businessTrip, CancellationToken cancellationToken);
    Task<IReadOnlyDictionary<long, BusinessTripPermissionsVm>> GetPermissions(
        IReadOnlyCollection<BusinessTrip> businessTrips, CancellationToken cancellationToken);
}