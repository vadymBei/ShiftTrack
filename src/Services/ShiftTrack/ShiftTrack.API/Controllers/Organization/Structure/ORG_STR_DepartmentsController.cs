using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.CreateDepartment;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.DeleteDepartment;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.UpdateDepartment;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetDepartmentById;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetDepartmentsByRoles;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetDepartmentsByUnitId;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetGroupedDepartmentsByUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Structure;

[Authorize]
[Route("api/shift-track/organization/structure/departments")]
public class ORG_STR_DepartmentsController : ApiController
{
    [HttpPost]
    public Task<DepartmentVm> Create(DepartmentToCreateDto data)
        => Mediator.Send(new CreateDepartmentCommand(data));

    [HttpPut]
    public Task<DepartmentVm> UpdateDepartment(DepartmentToUpdateDto data)
        => Mediator.Send(new UpdateDepartmentCommand(data));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(long id)
    {
        await Mediator.Send(new DeleteDepartmentCommand(id));

        return Ok();
    }

    [HttpGet("by-unitId/{unitId}")]
    public Task<IEnumerable<DepartmentVm>> GetDepartments(long unitId)
        => Mediator.Send(new GetDepartmentsByUnitIdQuery(unitId));

    [HttpGet("by-id/{id}")]
    public Task<DepartmentVm> GetDepartmentById(long id)
        => Mediator.Send(new GetDepartmentByIdQuery(id));

    [HttpGet("grouped/by-unit")]
    public Task<IEnumerable<GroupedDepartmentsByUnitVm>> GetGroupedDepartmentsByUnit()
        => Mediator.Send(new GetGroupedDepartmentsByUnitQuery());
    
    [HttpGet("by-roles/{unitId}")]
    public Task<IEnumerable<DepartmentVm>> GetDepartmentsByRoles(long unitId)
        => Mediator.Send(new GetDepartmentsByRolesQuery(unitId));
}