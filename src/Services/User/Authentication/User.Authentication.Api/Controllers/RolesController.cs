using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.CQRS.Controllers;
using User.Authentication.Application.Features.oAuth.Common.Dtos;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;
using User.Authentication.Application.Features.oAuth.Roles.Commands.CreateRole;
using User.Authentication.Application.Features.oAuth.Roles.Queries.GetRoles;

namespace User.Authentication.Api.Controllers;

[Authorize]
[Route("api/user/authentication/roles")]
public class RolesController : ApiController
{
    [HttpPost]
    public async Task<RoleVm> CreateRole(RoleToCreateDto commandData)
        => await Mediator.Invoke(new CreateRoleCommand(commandData));

    [HttpGet]
    public async Task<IEnumerable<RoleVm>> GetRoles()
        => await Mediator.Invoke(new GetRolesQuery());
}