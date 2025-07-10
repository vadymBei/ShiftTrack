using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Structure.Departments.Commands.CreateDepartment;
using ShiftTrack.Application.Features.Organization.Structure.Departments.Commands.DeleteDepartment;
using ShiftTrack.Application.Features.Organization.Structure.Departments.Commands.UpdateDepartment;
using ShiftTrack.Application.Features.Organization.Structure.Departments.Queries.GetDepartmentById;
using ShiftTrack.Application.Features.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId;
using ShiftTrack.Application.Features.Organization.Structure.Departments.Queries.GetGroupedDepartmentsByUnit;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Structure;

[Authorize]
[Route("api/shift-track/organization/structure/departments")]
public class ORG_STR_DepartmentsController : ApiController
{
    [HttpPost]
    public Task<DepartmentVm> Create(CreateDepartmentCommand command)
        => Mediator.Invoke(command);

    [HttpPut]
    public Task<DepartmentVm> UpdateDepartment(UpdateDepartmentCommand command)
        => Mediator.Invoke(command);

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(long id)
    {
        await Mediator.Invoke(new DeleteDepartmentCommand(id));

        return Ok();
    }

    [HttpGet("by-unitId/{unitId}")]
    public Task<IEnumerable<DepartmentVm>> GetDepartments(long unitId)
        => Mediator.Invoke(new GetDepartmentsByUnitIdQuery(unitId));

    [HttpGet("by-id/{id}")]
    public Task<DepartmentVm> GetDepartmentById(long id)
        => Mediator.Invoke(new GetDepartmentByIdQuery(id));

    [HttpGet("grouped/by-unit")]
    public Task<IEnumerable<GroupedDepartmentsByUnitVm>> GetGroupedDepartmentsByUnit()
        => Mediator.Invoke(new GetGroupedDepartmentsByUnitQuery());
}