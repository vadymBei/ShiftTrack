using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.User.EmployeeRoles.Commands.CreateEmployeeRole;
using ShiftTrack.Kernel.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/user/employee-roles")]
public class SYS_USR_EmployeeRolesController : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeRole(CreateEmployeeRoleCommand command)
    {
        await Mediator.Send(command);

        return Ok();
    }
}