using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.CreatePosition;
using ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.DeletePosition;
using ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.UpdatePosition;
using ShiftTrack.Application.Features.Organization.Structure.Positions.Queries.GetPositionById;
using ShiftTrack.Application.Features.Organization.Structure.Positions.Queries.GetPositions;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Structure;

[Authorize]
[Route("api/shift-track/organization/structure/positions")]
public class ORG_STR_PositionsController : ApiController
{
    [HttpPost]
    public async Task<PositionVm> CreatePosition(CreatePositionCommand command)
        => await Mediator.Invoke(command);

    [HttpPut]
    public async Task<PositionVm> UpdatePosition(UpdatePositionCommand command)
        => await Mediator.Invoke(command);

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePosition(long id)
    {
        await Mediator.Invoke(new DeletePositionCommand(id));

        return Ok();
    }

    [HttpGet]
    public async Task<IEnumerable<PositionVm>> GetPositions()
        => await Mediator.Invoke(new GetPositionsQuery());

    [HttpGet("{id}")]
    public async Task<PositionVm> GetPositionById(long id)
        => await Mediator.Invoke(new GetPositionByIdQuery(id));
}