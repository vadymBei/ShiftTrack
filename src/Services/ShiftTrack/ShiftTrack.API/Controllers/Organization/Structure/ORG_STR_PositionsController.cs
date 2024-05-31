using Kernel.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.CreatePosition;
using ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.DeletePosition;
using ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.UpdatePosition;
using ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositionById;
using ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositions;

namespace ShiftTrack.API.Controllers.Organization.Structure
{
    [Authorize]
    [Route("api/shift-track/organization/structure/positions")]
    public class ORG_STR_PositionsController : ApiController
    {
        [HttpPost]
        public async Task<PositionVM> CreatePosition(CreatePositionCommand command)
            => await Mediator.Send(command);

        [HttpPut]
        public async Task<PositionVM> UpdatePosition(UpdatePositionCommand command)
            => await Mediator.Send(command);

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(long id)
        {
            var idToken = await HttpContext.GetTokenAsync("Bearer");

            await Mediator.Send(new DeletePositionCommand(id));

            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<PositionVM>> GetPositions()
            => await Mediator.Send(new GetPositionsQuery());

        [HttpGet("{id}")]
        public async Task<PositionVM> GetPositionById(long id)
            => await Mediator.Send(new GetPositionByIdQuery(id));
    }
}
