using User.Authentication.Domain.Models.oAuth.Models;

namespace User.Authentication.Application.Modules.oAuth.Common.Interfaces;

public interface ITokenService
{
    Task<Token> GenerateToken(string login, string password, CancellationToken cancellationToken);
    Task<Token> RefreshToken(string refreshToken, CancellationToken cancellationToken);
}