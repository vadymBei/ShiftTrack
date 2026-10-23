using ShiftTrack.Application.Modules.Booking.Requests.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Requests.UseCases.Queries.GetPendingApprovalsCount;

public record GetPendingApprovalsCountQuery : IRequest<PendingApprovalsCountVm>;
