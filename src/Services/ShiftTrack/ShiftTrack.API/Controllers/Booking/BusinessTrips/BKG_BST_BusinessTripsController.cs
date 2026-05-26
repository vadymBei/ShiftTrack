using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.ApproveBusinessTrip;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.CreateBusinessTrip;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.DeleteBusinessTrip;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.RejectBusinessTrip;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.UpdateBusinessTrip;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.GetBusinessTripById;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.GetBusinessTrips;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Booking.BusinessTrips;

[Authorize]
[Route("api/shift-track/booking/business-trips")]
public class BKG_BST_BusinessTripsController : ApiController
{
    [HttpPost]
    public async Task<BusinessTripVm> Create([FromBody] BusinessTripToCreateDto request)
        => await Mediator.Invoke(new CreateBusinessTripCommand(request));

    [HttpPut]
    public async Task<BusinessTripVm> Update([FromBody] BusinessTripToUpdateDto request)
        => await Mediator.Invoke(new UpdateBusinessTripCommand(request));

    [HttpPut("approve/{id:long}")]
    public async Task<BusinessTripVm> Approve(long id)
        => await Mediator.Invoke(new ApproveBusinessTripCommand(id));
    
    [HttpPut("reject/{id:long}")]
    public async Task<BusinessTripVm> Reject(long id)
        => await Mediator.Invoke(new RejectBusinessTripCommand(id));
    
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await Mediator.Invoke(new DeleteBusinessTripCommand(id));

        return Ok();
    }

    [HttpGet("filtered")]
    public async Task<IEnumerable<BusinessTripVm>> GetFiltered([FromQuery] BusinessTripFilterDto filter)
        => await Mediator.Invoke(new GetBusinessTripsQuery(filter));
    
    [HttpGet("{id:long}")]
    public async Task<BusinessTripVm> GetById(long id)
        => await Mediator.Invoke(new GetBusinessTripByIdQuery(id));
}