using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;
using ShiftTrack.Core.Application.System.Auth.Tokens.Commands.GenerateToken;
using ShiftTrack.Core.Application.System.Auth.Tokens.Commands.RefreshToken;
using ShiftTrack.Kernel.Controllers;

namespace ShiftTrack.API.Controllers.System.Auth
{
    [AllowAnonymous]
    [Route("api/shift-track/system/auth/tokens")]
    public class SYS_AUTH_TokensController : ApiController
    {
        [HttpPost("generate")]
        public Task<TokenVM> Generate(GenerateTokenDto data)
            => Mediator.Send(new GenerateTokenCommand(data));

        [HttpPost("refresh")]
        public Task<TokenVM> Refresh(RefreshTokenDto data)
            => Mediator.Send(new RefreshTokenCommand(data));
    }
}
