using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.DeleteUnit;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.UpdateUnit;
using ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnitById;
using ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnits;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Structure;

[Authorize]
[Route("api/shift-track/organization/structure/units")]
public class ORG_STR_UnitsController : ApiController
{
    [HttpPost]
    public async Task<UnitVM> CreateUnit(CreateUnitCommand command)
        => await Mediator.Invoke(command);

    [HttpPut]
    public async Task<UnitVM> UpdateUnit(UpdateUnitCommand command)
        => await Mediator.Invoke(command);

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUnit(long id)
    {
        await Mediator.Invoke(new DeleteUnitCommand(id));

        return Ok();
    }

    [HttpGet]
    public async Task<IEnumerable<UnitVM>> GetUnits()
        => await Mediator.Invoke(new GetUnitsQuery());

    [HttpGet("{id}")]
    public async Task<UnitVM> GetUnitById(long id)
        => await Mediator.Invoke(new GetUnitByIdQuery(id));
}