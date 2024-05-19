using Kernel.Controllers;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.DeleteShift;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShiftById;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShifts;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.UpdateShift;

namespace ShiftTrack.API.Controllers.Timesheet
{
    [Route("api/timesheet/shifts")]
    public class ShiftsController : ApiController
    {
        [HttpPost]
        public async Task<ShiftVM> CreateShift(CreateShiftCommand command)
            => await Mediator.Send(command);

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(long id)
        {
            await Mediator.Send(new DeleteShiftCommand()
            {
                Id = id
            });

            return Ok();
        }

        [HttpPut]
        public async Task<ShiftVM> UpdateShift(UpdateShiftCommand command)
            => await Mediator.Send(command);

        [HttpGet]
        public async Task<IEnumerable<ShiftVM>> GetShifts()
            => await Mediator.Send(new GetShiftsQuery());

        [HttpGet("{id}")]
        public async Task<ShiftVM> GetShiftById(long id)
            => await Mediator.Send(new GetShiftByIdQuery()
            {
                Id = id
            });
    }
}
