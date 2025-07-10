using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.Roles.Queries.GetRoles;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/user/roles")]
public class SYS_USR_RolesController : ApiController
{
    [HttpGet]
    public async Task<IEnumerable<RoleVm>> GetRoles()
        => await Mediator.Invoke(new GetRolesQuery());
}