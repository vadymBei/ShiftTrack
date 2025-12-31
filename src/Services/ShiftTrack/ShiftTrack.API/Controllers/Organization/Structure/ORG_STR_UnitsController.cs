using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Application.Features.Organization.Structure.Units.Commands.DeleteUnit;
using ShiftTrack.Application.Features.Organization.Structure.Units.Commands.UpdateUnit;
using ShiftTrack.Application.Features.Organization.Structure.Units.Queries.GetUnitById;
using ShiftTrack.Application.Features.Organization.Structure.Units.Queries.GetUnits;
using ShiftTrack.Application.Features.Organization.Structure.Units.Queries.GetUnitsByRoles;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Structure;

[Authorize]
[Route("api/shift-track/organization/structure/units")]
public class ORG_STR_UnitsController : ApiController
{
    [HttpPost]
    public async Task<UnitVm> CreateUnit(CreateUnitCommand command)
        => await Mediator.Invoke(command);

    [HttpPut]
    public async Task<UnitVm> UpdateUnit(UpdateUnitCommand command)
        => await Mediator.Invoke(command);

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUnit(long id)
    {
        await Mediator.Invoke(new DeleteUnitCommand(id));

        return Ok();
    }

    [HttpGet]
    public async Task<IEnumerable<UnitVm>> GetUnits()
        => await Mediator.Invoke(new GetUnitsQuery());

    [HttpGet("by-roles")]
    public async Task<IEnumerable<UnitVm>> GetUnitsByRoles()
        => await Mediator.Invoke(new GetUnitsByRolesQuery());
    
    [HttpGet("{id}")]
    public async Task<UnitVm> GetUnitById(long id)
        => await Mediator.Invoke(new GetUnitByIdQuery(id));
}