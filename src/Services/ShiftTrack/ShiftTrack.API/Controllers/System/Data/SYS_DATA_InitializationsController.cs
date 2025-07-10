using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.System.Data.Initializations.Commands.InitializeSysAdmin;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.Data;

[AllowAnonymous]
[Route("api/shift-track/system/data/initializations")]
public class SYS_DATA_InitializationsController : ApiController
{
    [HttpPost("sys-admin")]
    public async Task<IActionResult> InitializeSystemAdmin()
    {
        await Mediator.Invoke(new InitializeSysAdminCommand());
        
        return Ok();
    }
}