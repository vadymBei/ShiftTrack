using Microsoft.Extensions.Options;
using User.Authentication.Application.Features.oAuth.Common.Dtos;
using User.Authentication.Application.Features.oAuth.Common.Interfaces;
using User.Authentication.Domain.Features.oAuth.Models;
using User.Authentication.Domain.Features.oAuth.Options;

namespace User.Authentication.Application.Features.oAuth.Common.Services;

public class TokenService(
    ITokenRepository tokenRepository,
    IOptions<AuthClientOptions> authClientOptions)
    : ITokenService
{
    public Task<Token> GenerateToken(string login, string password, CancellationToken cancellationToken)
    {
        return tokenRepository.GenerateToken(
            new TokenRequestDto()
            {
                Client = authClientOptions.Value.Client,
                Scope = authClientOptions.Value.Scope,
                ClientSecret = authClientOptions.Value.ClientSecret,
                Login = login,
                Password = password
            },
            cancellationToken);
    }

    public Task<Token> RefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        return tokenRepository.RefreshToken(
            new TokenRequestDto()
            {
                Client = authClientOptions.Value.Client,
                ClientSecret = authClientOptions.Value.ClientSecret
            },
            refreshToken,
            cancellationToken);
    }
}