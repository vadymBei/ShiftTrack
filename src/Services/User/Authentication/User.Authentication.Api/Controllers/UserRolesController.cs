using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.CQRS.Controllers;
using User.Authentication.Application.Modules.oAuth.Common.Dtos;
using User.Authentication.Application.Modules.oAuth.UserRoles.Commands.CreateUserRole;

namespace User.Authentication.Api.Controllers;

[Authorize(AuthenticationSchemes = "BasicAuthentication")]
[Route("api/user/authentication/user-roles")]
public class UserRolesController : ApiController
{
    [HttpPost]
    public async Task CreateUserRole(UserRoleToCreateDto commandData)
        => await Mediator.Invoke(new CreateUserRoleCommand(commandData));
}