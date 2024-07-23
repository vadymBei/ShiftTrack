using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.Controllers;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.ViewModels;
using User.Authentication.Core.Application.Users.Commands.ChangePassword;
using User.Authentication.Core.Application.Users.Commands.CreateUser;
using User.Authentication.Core.Application.Users.Commands.UpdateUser;

namespace User.Authentication.Api.Controllers
{
    [Authorize]
    [Route("api/user/authentication/users")]
    public class UsersController : ApiController
    {
        [HttpPost("register")]
        public async Task<UserVM> CreateUser(CreateUserCommand command)
            => await Mediator.Send(command);

        [HttpPut]
        public async Task<UserVM> UpdateUser(UpdateUserCommand command)
            => await Mediator.Send(command);

        [HttpPost("change-password")]
        public async Task<TokenVM> ChangePassword(ChangePasswordDto commandData)
            => await Mediator.Send(new ChangePasswordCommand(commandData));
    }
}
