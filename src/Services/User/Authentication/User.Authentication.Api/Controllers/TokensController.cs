using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.CQRS.Controllers;
using User.Authentication.Core.Application.Common.ViewModels;
using User.Authentication.Core.Application.Tokens.Commands.GenerateToken;
using User.Authentication.Core.Application.Tokens.Commands.RefreshToken;

namespace User.Authentication.Api.Controllers;

[Authorize]
[Route("api/user/authentication/tokens")]
public class Tokens : ApiController
{
    [HttpPost("generate")]
    public async Task<TokenVM> GenerateToken(GenerateTokenCommand command)
        => await Mediator.Invoke(command);

    [HttpPost("refresh")]
    public async Task<TokenVM> RefreshToken(RefreshTokenCommand command)
        => await Mediator.Invoke(command);
}