using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.System.Auth.Common.Dtos;
using ShiftTrack.Application.Features.System.Auth.Common.ViewModels;
using ShiftTrack.Application.Features.System.Auth.Tokens.Commands.GenerateToken;
using ShiftTrack.Application.Features.System.Auth.Tokens.Commands.RefreshToken;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.Auth;

[AllowAnonymous]
[Route("api/shift-track/system/auth/tokens")]
public class SYS_AUTH_TokensController : ApiController
{
    [HttpPost("generate")]
    public Task<TokenVm> Generate(GenerateTokenDto data)
        => Mediator.Invoke(new GenerateTokenCommand(data));

    [HttpPost("refresh")]
    public Task<TokenVm> Refresh(RefreshTokenDto data)
        => Mediator.Invoke(new RefreshTokenCommand(data));
}