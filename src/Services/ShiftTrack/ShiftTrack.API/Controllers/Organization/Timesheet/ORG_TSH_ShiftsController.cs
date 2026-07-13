using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.CreateShift;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.DeleteShift;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.UpdateShift;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Queries.GetShiftById;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Queries.GetShifts;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Timesheet;

[Authorize]
[Route("api/shift-track/timesheet/shifts")]
public class ORG_TSH_ShiftsController : ApiController
{
    [HttpPost]
    public async Task<ShiftVm> CreateShift(ShiftToCreateDto data)
        => await Mediator.Send(new CreateShiftCommand(data));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShift(long id)
    {
        await Mediator.Send(new DeleteShiftCommand(id));

        return Ok();
    }

    [HttpPut]
    public async Task<ShiftVm> UpdateShift(ShiftToUpdateDto data)
        => await Mediator.Send(new UpdateShiftCommand(data));

    [HttpGet]
    public async Task<IEnumerable<ShiftVm>> GetShifts()
        => await Mediator.Send(new GetShiftsQuery());

    [HttpGet("{id}")]
    public async Task<ShiftVm> GetShiftById(long id)
        => await Mediator.Send(new GetShiftByIdQuery(id));
}