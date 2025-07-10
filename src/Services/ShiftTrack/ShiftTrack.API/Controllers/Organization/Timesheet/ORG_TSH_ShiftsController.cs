using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Commands.CreateShift;
using ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Commands.DeleteShift;
using ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Commands.UpdateShift;
using ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Queries.GetShiftById;
using ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Queries.GetShifts;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Timesheet;

[Authorize]
[Route("api/shift-track/timesheet/shifts")]
public class ORG_TSH_ShiftsController : ApiController
{
    [HttpPost]
    public async Task<ShiftVm> CreateShift(CreateShiftCommand command)
        => await Mediator.Invoke(command);

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShift(long id)
    {
        await Mediator.Invoke(new DeleteShiftCommand(id));

        return Ok();
    }

    [HttpPut]
    public async Task<ShiftVm> UpdateShift(UpdateShiftCommand command)
        => await Mediator.Invoke(command);

    [HttpGet]
    public async Task<IEnumerable<ShiftVm>> GetShifts()
        => await Mediator.Invoke(new GetShiftsQuery());

    [HttpGet("{id}")]
    public async Task<ShiftVm> GetShiftById(long id)
        => await Mediator.Invoke(new GetShiftByIdQuery(id));
}