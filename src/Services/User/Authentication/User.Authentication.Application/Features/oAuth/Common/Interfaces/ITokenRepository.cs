using User.Authentication.Application.Features.oAuth.Common.Dtos;
using User.Authentication.Domain.Features.oAuth.Models;

namespace User.Authentication.Application.Features.oAuth.Common.Interfaces;

public interface ITokenRepository
{
    Task<Token> GenerateToken(TokenRequestDto dto, CancellationToken cancellationToken);
    Task<Token> RefreshToken(TokenRequestDto dto, string refreshToken, CancellationToken cancellationToken);
}