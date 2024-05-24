using Kernel.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Authentication.Core.Application.Common.ViewModels;
using User.Authentication.Core.Application.Users.Commands.CreateUser;

namespace User.Authentication.Api.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UsersController : ApiController
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<UserVM> CreateUser(CreateUserCommand command)
            => await Mediator.Send(command);
    }
}
