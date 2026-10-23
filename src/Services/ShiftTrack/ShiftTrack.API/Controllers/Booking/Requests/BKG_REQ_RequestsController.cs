using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Booking.Requests.UseCases.Queries.GetMyRequests;
using ShiftTrack.Application.Modules.Booking.Requests.UseCases.Queries.GetPendingApprovals;
using ShiftTrack.Application.Modules.Booking.Requests.UseCases.Queries.GetPendingApprovalsCount;
using ShiftTrack.Application.Modules.Booking.Requests.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Booking.Requests;

[Authorize]
[Route("api/shift-track/booking")]
public class BKG_REQ_RequestsController : ApiController
{
    [HttpGet("requests/my")]
    public Task<MyBookingRequestsVm> GetMyRequests([FromQuery] int take = 5)
        => Mediator.Send(new GetMyRequestsQuery(take));

    [HttpGet("approvals/pending")]
    public Task<IEnumerable<PendingApprovalVm>> GetPendingApprovals([FromQuery] int take = 5)
        => Mediator.Send(new GetPendingApprovalsQuery(take));

    [HttpGet("approvals/pending/count")]
    public Task<PendingApprovalsCountVm> GetPendingApprovalsCount()
        => Mediator.Send(new GetPendingApprovalsCountQuery());
}
