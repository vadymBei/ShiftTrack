using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Kernel.CQRS.Controllers;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;
using User.Authentication.Application.Modules.oAuth.Tokens.Commands.GenerateToken;
using User.Authentication.Application.Modules.oAuth.Tokens.Commands.RefreshToken;

namespace User.Authentication.Api.Controllers;

[Authorize(AuthenticationSchemes = "BasicAuthentication")]
[Route("api/user/authentication/tokens")]
public class Tokens : ApiController
{
    [HttpPost("generate")]
    public async Task<TokenVm> GenerateToken(GenerateTokenCommand command)
        => await Mediator.Send(command);

    [HttpPost("refresh")]
    public async Task<TokenVm> RefreshToken(RefreshTokenCommand command)
        => await Mediator.Send(command);
}