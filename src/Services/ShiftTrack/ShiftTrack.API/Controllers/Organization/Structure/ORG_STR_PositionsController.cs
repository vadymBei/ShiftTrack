using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.CreatePosition;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.DeletePosition;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.UpdatePosition;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Queries.GetPositionById;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Queries.GetPositions;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Structure;

[Authorize]
[Route("api/shift-track/organization/structure/positions")]
public class ORG_STR_PositionsController : ApiController
{
    [HttpPost]
    public async Task<PositionVm> CreatePosition(PositionToCreateDto data)
        => await Mediator.Send(new CreatePositionCommand(data));

    [HttpPut]
    public async Task<PositionVm> UpdatePosition(PositionToUpdateDto data)
        => await Mediator.Send(new UpdatePositionCommand(data));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePosition(long id)
    {
        await Mediator.Send(new DeletePositionCommand(id));

        return Ok();
    }

    [HttpGet]
    public async Task<IEnumerable<PositionVm>> GetPositions()
        => await Mediator.Send(new GetPositionsQuery());

    [HttpGet("{id}")]
    public async Task<PositionVm> GetPositionById(long id)
        => await Mediator.Send(new GetPositionByIdQuery(id));
}