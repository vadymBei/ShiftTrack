using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.Requests.ViewModels;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Requests.UseCases.Queries.GetPendingApprovalsCount;

public class GetPendingApprovalsCountQueryHandler(
    IVacationService vacationService,
    IBusinessTripService businessTripService) : IRequestHandler<GetPendingApprovalsCountQuery, PendingApprovalsCountVm>
{
    public async Task<PendingApprovalsCountVm> Handle(GetPendingApprovalsCountQuery request,
        CancellationToken cancellationToken = default)
    {
        return new PendingApprovalsCountVm
        {
            Vacations = await vacationService.CountPendingApprovals(cancellationToken),
            BusinessTrips = await businessTripService.CountPendingApprovals(cancellationToken)
        };
    }
}
