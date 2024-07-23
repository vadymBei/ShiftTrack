using MediatR;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Tokens.Commands.RefreshToken
{
    public record RefreshTokenCommand(
        string RefreshToken) : IRequest<TokenVM>;
}
