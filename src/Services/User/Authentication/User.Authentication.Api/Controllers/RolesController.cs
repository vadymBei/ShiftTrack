using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.CQRS.Controllers;
using User.Authentication.Application.Modules.oAuth.Common.Dtos;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;
using User.Authentication.Application.Modules.oAuth.Roles.Commands.CreateRole;
using User.Authentication.Application.Modules.oAuth.Roles.Queries.GetRoles;

namespace User.Authentication.Api.Controllers;

[Authorize(AuthenticationSchemes = "BasicAuthentication")]
[Route("api/user/authentication/roles")]
public class RolesController : ApiController
{
    [HttpPost]
    public async Task<RoleVm> CreateRole(RoleToCreateDto commandData)
        => await Mediator.Send(new CreateRoleCommand(commandData));

    [HttpGet]
    public async Task<IEnumerable<RoleVm>> GetRoles()
        => await Mediator.Send(new GetRolesQuery());
}