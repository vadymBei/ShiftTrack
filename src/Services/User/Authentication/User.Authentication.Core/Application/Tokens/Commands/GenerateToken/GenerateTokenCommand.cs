using MediatR;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Tokens.Commands.GenerateToken
{
    public record GenerateTokenCommand(
        string Login, 
        string Password) : IRequest<TokenVM>;
}
