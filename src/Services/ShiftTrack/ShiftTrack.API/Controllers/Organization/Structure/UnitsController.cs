using Kernel.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.DeleteUnit;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.UpdateUnit;
using ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnitById;
using ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnits;

namespace ShiftTrack.API.Controllers.Organization.Structure
{
    [Authorize]
    [Route("api/shift-track/organization/structure/units")]
    public class UnitsController : ApiController
    {
        [HttpPost]
        public async Task<UnitVM> CreateUnit(CreateUnitCommand command)
            => await Mediator.Send(command);

        [HttpPut]
        public async Task<UnitVM> UpdateUnit(UpdateUnitCommand command)
            => await Mediator.Send(command);

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(long id)
        {
            await Mediator.Send(new DeleteUnitCommand()
            {
                Id = id
            });

            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<UnitVM>> GetUnits()
            => await Mediator.Send(new GetUnitsQuery());

        [HttpGet("{id}")]
        public async Task<UnitVM> GetUnitById(long id)
            => await Mediator.Send(new GetUnitByIdQuery()
            {
                Id = id
            });
    }
}
