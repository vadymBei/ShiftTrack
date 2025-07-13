using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.CQRS.Controllers;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;
using User.Authentication.Application.Features.oAuth.Tokens.Commands.GenerateToken;
using User.Authentication.Application.Features.oAuth.Tokens.Commands.RefreshToken;

namespace User.Authentication.Api.Controllers;

[Authorize]
[Route("api/user/authentication/tokens")]
public class Tokens : ApiController
{
    [HttpPost("generate")]
    public async Task<TokenVm> GenerateToken(GenerateTokenCommand command)
        => await Mediator.Invoke(command);

    [HttpPost("refresh")]
    public async Task<TokenVm> RefreshToken(RefreshTokenCommand command)
        => await Mediator.Invoke(command);
}