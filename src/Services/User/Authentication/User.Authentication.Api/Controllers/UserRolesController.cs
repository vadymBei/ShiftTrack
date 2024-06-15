using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.Controllers;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.UserRoles.Commands.CreateUserRole;

namespace User.Authentication.Api.Controllers
{
    [Authorize]
    [Route("api/user/authentication/user-roles")]
    public class UserRolesController : ApiController
    {
        [HttpPost]
        public async Task CreateUserRole(UserRoleToCreateDto commandData)
            => await Mediator.Send(new CreateUserRoleCommand(commandData));
    }
}
