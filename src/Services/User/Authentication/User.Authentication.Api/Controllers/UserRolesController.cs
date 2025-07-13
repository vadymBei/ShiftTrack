using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.CQRS.Controllers;
using User.Authentication.Application.Features.oAuth.Common.Dtos;
using User.Authentication.Application.Features.oAuth.UserRoles.Commands.CreateUserRole;

namespace User.Authentication.Api.Controllers;

[Authorize]
[Route("api/user/authentication/user-roles")]
public class UserRolesController : ApiController
{
    [HttpPost]
    public async Task CreateUserRole(UserRoleToCreateDto commandData)
        => await Mediator.Invoke(new CreateUserRoleCommand(commandData));
}