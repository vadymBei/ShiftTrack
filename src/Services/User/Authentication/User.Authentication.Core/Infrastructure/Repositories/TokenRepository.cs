using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using ShiftTrack.Authentication.Options;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Exceptions;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Domain.Models.OAuth;

namespace User.Authentication.Core.Infrastructure.Repositories
{
    internal class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;

        public TokenRepository(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Token> GenerateToken(TokenRequestDto dto, CancellationToken cancellationToken)
        {
            var authOptions = _configuration
                .GetSection("AuthenticationOptions")
                .Get<ServiceAuthenticationOptions>();

            using var httpClient = new HttpClient();

            var responce = await httpClient.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = authOptions.AuthServer.Authority + "/connect/token",
                    ClientId = dto.Client,
                    Scope = dto.Scope,
                    UserName = dto.Login,
                    Password = dto.Password,
                    ClientSecret = dto.ClientSecret
                },
                cancellationToken);

            if (responce.IsError)
            {
                throw new InvalidCredentialsException(dto.Login, responce.Error);
            }

            return new Token()
            {
                AccessToken = responce.AccessToken,
                RefreshToken = responce.RefreshToken,
                TokenType = responce.TokenType,
                ExpiresIn = responce.ExpiresIn
            };
        }

        public async Task<Token> RefreshToken(TokenRequestDto dto, string refreshToken, CancellationToken cancellationToken)
        {
            var authOptions = _configuration
                .GetSection("AuthenticationOptions")
                .Get<ServiceAuthenticationOptions>();

            using var httpClient = new HttpClient();

            var response = await httpClient.RequestRefreshTokenAsync(
                new RefreshTokenRequest()
                {
                    Address = authOptions.AuthServer.Authority + "/connect/token",
                    ClientId = dto.Client,
                    ClientSecret = dto.ClientSecret,
                    RefreshToken = refreshToken
                },
                cancellationToken);

            if (response.IsError)
            {
                throw new InvalidCredentialsException(dto.Login, response.Error);
            }

            return new Token()
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                TokenType = response.TokenType,
                ExpiresIn = response.ExpiresIn
            };
        }
    }
}
