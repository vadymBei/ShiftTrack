using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.EmployeeRoles.Commands.CreateEmployeeRole;
using ShiftTrack.Core.Application.System.User.EmployeeRoles.Commands.DeleteEmployeeRole;
using ShiftTrack.Core.Application.System.User.EmployeeRoles.Queries.GetEmployeeRoleById;
using ShiftTrack.Core.Application.System.User.EmployeeRoles.Queries.GetEmployeeRolesByEmployeeId;
using ShiftTrack.Kernel.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/user/employee-roles")]
public class SYS_USR_EmployeeRolesController : ApiController
{
    [HttpPost]
    public async Task<EmployeeRoleVm> CreateEmployeeRole([FromBody] EmployeeRoleToCreateDto dto)
        => await Mediator.Send(new CreateEmployeeRoleCommand(dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeRole(long id)
    {
        await Mediator.Send(new DeleteEmployeeRoleCommand(id));

        return Ok();
    }

    [HttpGet("{id}")]
    public Task<EmployeeRoleVm> GetEmployeeRoleById(long id)
        => Mediator.Send(new GetEmployeeRoleByIdQuery(id));

    [HttpGet("by-employeeId/{employeeId}")]
    public Task<IEnumerable<EmployeeRoleVm>> GetEmployeeRolesByEmployeeId(long employeeId)
        => Mediator.Send(new GetEmployeeRolesByEmployeeIdQuery(employeeId));
}