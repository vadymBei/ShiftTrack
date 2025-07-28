using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Application.Features.Booking.Vacations.Commands.CreateVacation;
using ShiftTrack.Application.Features.Booking.Vacations.Commands.DeleteVacation;
using ShiftTrack.Application.Features.Booking.Vacations.Commands.UpdateVacation;
using ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacationById;
using ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacations;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Booking.Vacations;

[Authorize]
[Route("api/shift-track/booking/vacations")]
public class BKG_VCN_VacationsController : ApiController
{
    [HttpPost]
    public Task<VacationVm> CreateVacation(CreateVacationCommand command)
        => Mediator.Invoke(command);

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVacation(long id)
    {
        await Mediator.Invoke(new DeleteVacationCommand(id));

        return Ok();
    }

    [HttpGet]
    public Task<IEnumerable<VacationVm>> GetVacations([FromQuery] GetVacationsQuery query)
        => Mediator.Invoke(query);

    [HttpGet("{id}")]
    public Task<VacationVm> GetVacationById(long id)
        => Mediator.Invoke(new GetVacationByIdQuery(id));

    [HttpPut]
    public Task<VacationVm> UpdateVacation([FromBody] UpdateVacationCommand command)
        => Mediator.Invoke(command);
}