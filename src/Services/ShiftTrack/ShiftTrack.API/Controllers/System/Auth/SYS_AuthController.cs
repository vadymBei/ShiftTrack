using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;
using ShiftTrack.Core.Application.System.Auth.Tokens.Commands.GenerateToken;
using ShiftTrack.Core.Application.System.Auth.Tokens.Commands.RefreshToken;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Employees.Commands.ChangePassword;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.Auth;

[AllowAnonymous]
[Route("api/shift-track/system/auth")]
public class SYS_AuthController : ApiController
{
    [HttpPost("token/generate")]
    public Task<TokenVM> Generate(GenerateTokenDto data)
        => Mediator.Invoke(new GenerateTokenCommand(data));

    [HttpPost("token/refresh")]
    public Task<TokenVM> Refresh(RefreshTokenDto data)
        => Mediator.Invoke(new RefreshTokenCommand(data));
    
    [HttpPost("password/change")]
    public async Task<TokenVM> ChangePassword(ChangeEmployeePasswordDto commandData)
        => await Mediator.Invoke(new ChangePasswordCommand(commandData));
}