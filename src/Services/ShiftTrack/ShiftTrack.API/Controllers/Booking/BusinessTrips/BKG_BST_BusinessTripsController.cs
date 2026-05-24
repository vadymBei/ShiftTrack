using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.CreateBusinessTrip;
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
}