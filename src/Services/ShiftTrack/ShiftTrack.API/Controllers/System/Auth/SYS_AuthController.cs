using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.System.Auth.Account.Commands.ChangePassword;
using ShiftTrack.Application.Features.System.Auth.Common.Dtos;
using ShiftTrack.Application.Features.System.Auth.Common.ViewModels;
using ShiftTrack.Application.Features.System.Auth.Tokens.Commands.GenerateToken;
using ShiftTrack.Application.Features.System.Auth.Tokens.Commands.RefreshToken;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.Auth;

[AllowAnonymous]
[Route("api/shift-track/system/auth")]
public class SYS_AuthController : ApiController
{
    [HttpPost("token/generate")]
    public Task<TokenVm> Generate(GenerateTokenDto data)
        => Mediator.Invoke(new GenerateTokenCommand(data));

    [HttpPost("token/refresh")]
    public Task<TokenVm> Refresh(RefreshTokenDto data)
        => Mediator.Invoke(new RefreshTokenCommand(data));
    
    [HttpPost("password/change")]
    public async Task<TokenVm> ChangePassword(ChangePasswordDto commandData)
        => await Mediator.Invoke(new ChangePasswordCommand(commandData));
}