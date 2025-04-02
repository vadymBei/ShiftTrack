using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.Roles.Queries.GetRoles;
using ShiftTrack.Kernel.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/user/roles")]
public class SYS_USR_RolesController : ApiController
{
    [HttpGet]
    public async Task<IEnumerable<RoleVM>> GetRoles()
        => await Mediator.Send(new GetRolesQuery());
}