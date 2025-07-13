using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.CQRS.Controllers;
using User.Authentication.Application.Features.oAuth.Account.Commands.ChangePassword;
using User.Authentication.Application.Features.oAuth.Account.Commands.Register;
using User.Authentication.Application.Features.oAuth.Account.Commands.UpdateAccount;
using User.Authentication.Application.Features.oAuth.Common.Dtos;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Api.Controllers;

[Authorize]
[Route("api/user/authentication/account")]
public class AccountController: ApiController
{
    [HttpPost("register")]
    public async Task<UserVm> CreateUser(RegisterCommand command)
        => await Mediator.Invoke(command);

    [HttpPut]
    public async Task<UserVm> UpdateUser(UpdateAccountCommand command)
        => await Mediator.Invoke(command);

    [HttpPost("change-password")]
    public async Task<TokenVm> ChangePassword(ChangePasswordDto commandData)
        => await Mediator.Invoke(new ChangePasswordCommand(commandData));
}