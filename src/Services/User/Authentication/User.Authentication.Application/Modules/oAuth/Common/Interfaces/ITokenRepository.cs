using User.Authentication.Application.Modules.oAuth.Common.Dtos;
using User.Authentication.Domain.Models.oAuth.Models;

namespace User.Authentication.Application.Modules.oAuth.Common.Interfaces;

public interface ITokenRepository
{
    Task<Token> GenerateToken(TokenRequestDto dto, CancellationToken cancellationToken);
    Task<Token> RefreshToken(TokenRequestDto dto, string refreshToken, CancellationToken cancellationToken);
}