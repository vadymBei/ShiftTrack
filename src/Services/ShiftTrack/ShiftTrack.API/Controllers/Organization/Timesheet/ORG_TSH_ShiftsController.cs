using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.DeleteShift;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.UpdateShift;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShiftById;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShifts;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Timesheet;

[Authorize]
[Route("api/shift-track/timesheet/shifts")]
public class ORG_TSH_ShiftsController : ApiController
{
    [HttpPost]
    public async Task<ShiftVM> CreateShift(CreateShiftCommand command)
        => await Mediator.Invoke(command);

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShift(long id)
    {
        await Mediator.Invoke(new DeleteShiftCommand(id));

        return Ok();
    }

    [HttpPut]
    public async Task<ShiftVM> UpdateShift(UpdateShiftCommand command)
        => await Mediator.Invoke(command);

    [HttpGet]
    public async Task<IEnumerable<ShiftVM>> GetShifts()
        => await Mediator.Invoke(new GetShiftsQuery());

    [HttpGet("{id}")]
    public async Task<ShiftVM> GetShiftById(long id)
        => await Mediator.Invoke(new GetShiftByIdQuery(id));
}