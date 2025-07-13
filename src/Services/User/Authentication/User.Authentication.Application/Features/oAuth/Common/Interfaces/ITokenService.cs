using User.Authentication.Domain.Features.oAuth.Models;

namespace User.Authentication.Application.Features.oAuth.Common.Interfaces;

public interface ITokenService
{
    Task<Token> GenerateToken(string login, string password, CancellationToken cancellationToken);
    Task<Token> RefreshToken(string refreshToken, CancellationToken cancellationToken);
}