using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.Controllers;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.ViewModels;
using User.Authentication.Core.Application.Roles.Commands.CreateRole;
using User.Authentication.Core.Application.Roles.Queries.GetRoles;

namespace User.Authentication.Api.Controllers
{
    [Authorize]
    [Route("api/user/authentication/roles")]
    public class RolesController : ApiController
    {
        [HttpPost]
        public async Task<RoleVM> CreateRole(RoleToCreateDto commandData)
            => await Mediator.Send(new CreateRoleCommand(commandData));

        [HttpGet]
        public async Task<IEnumerable<RoleVM>> GetRoles()
            => await Mediator.Send(new GetRolesQuery());
    }
}
