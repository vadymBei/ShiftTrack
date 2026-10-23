using ShiftTrack.Application.Modules.Booking.Requests.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Requests.UseCases.Queries.GetPendingApprovals;

public record GetPendingApprovalsQuery(
    int Take) : IRequest<IEnumerable<PendingApprovalVm>>;
