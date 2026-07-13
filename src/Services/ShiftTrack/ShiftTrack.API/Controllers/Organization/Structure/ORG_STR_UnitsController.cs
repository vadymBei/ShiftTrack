using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.CreateUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.DeleteUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.UpdateUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Queries.GetUnitById;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Queries.GetUnits;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Queries.GetUnitsByRoles;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Structure;

[Authorize]
[Route("api/shift-track/organization/structure/units")]
public class ORG_STR_UnitsController : ApiController
{
    [HttpPost]
    public async Task<UnitVm> CreateUnit(UnitToCreateDto data)
        => await Mediator.Send(new CreateUnitCommand(data));

    [HttpPut]
    public async Task<UnitVm> UpdateUnit(UnitToUpdateDto data)
        => await Mediator.Send(new UpdateUnitCommand(data));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUnit(long id)
    {
        await Mediator.Send(new DeleteUnitCommand(id));

        return Ok();
    }

    [HttpGet]
    public async Task<IEnumerable<UnitVm>> GetUnits()
        => await Mediator.Send(new GetUnitsQuery());

    [HttpGet("by-roles")]
    public async Task<IEnumerable<UnitVm>> GetUnitsByRoles()
        => await Mediator.Send(new GetUnitsByRolesQuery());

    [HttpGet("{id}")]
    public async Task<UnitVm> GetUnitById(long id)
        => await Mediator.Send(new GetUnitByIdQuery(id));
}