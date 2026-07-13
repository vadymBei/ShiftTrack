using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.System.Auth.Tokens.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Tokens.UseCases.Commands.GenerateToken;
using ShiftTrack.Application.Modules.System.Auth.Tokens.UseCases.Commands.RefreshToken;
using ShiftTrack.Application.Modules.System.Auth.Tokens.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.Auth;

[AllowAnonymous]
[Route("api/shift-track/system/auth/tokens")]
public class SYS_AUTH_TokensController : ApiController
{
    [HttpPost("generate")]
    public Task<TokenVm> Generate(GenerateTokenDto data)
        => Mediator.Send(new GenerateTokenCommand(data));

    [HttpPost("refresh")]
    public Task<TokenVm> Refresh(RefreshTokenDto data)
        => Mediator.Send(new RefreshTokenCommand(data));
}