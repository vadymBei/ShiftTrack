using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.System.User.Common.Dtos;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.EmployeeRoles.Commands.CreateEmployeeRole;
using ShiftTrack.Application.Features.System.User.EmployeeRoles.Commands.DeleteEmployeeRole;
using ShiftTrack.Application.Features.System.User.EmployeeRoles.Queries.GetEmployeeRoleById;
using ShiftTrack.Application.Features.System.User.EmployeeRoles.Queries.GetEmployeeRolesByEmployeeId;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/user/employee-roles")]
public class SYS_USR_EmployeeRolesController : ApiController
{
    [HttpPost]
    public async Task<EmployeeRoleVm> CreateEmployeeRole([FromBody] EmployeeRoleToCreateDto dto)
        => await Mediator.Invoke(new CreateEmployeeRoleCommand(dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeRole(long id)
    {
        await Mediator.Invoke(new DeleteEmployeeRoleCommand(id));

        return Ok();
    }

    [HttpGet("{id}")]
    public Task<EmployeeRoleVm> GetEmployeeRoleById(long id)
        => Mediator.Invoke(new GetEmployeeRoleByIdQuery(id));

    [HttpGet("by-employeeId/{employeeId}")]
    public Task<IEnumerable<EmployeeRoleVm>> GetEmployeeRolesByEmployeeId(long employeeId)
        => Mediator.Invoke(new GetEmployeeRolesByEmployeeIdQuery(employeeId));
}