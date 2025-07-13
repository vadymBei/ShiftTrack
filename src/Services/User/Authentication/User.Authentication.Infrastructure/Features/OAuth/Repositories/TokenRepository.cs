using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using ShiftTrack.Authentication.Options;
using User.Authentication.Application.Features.oAuth.Common.Dtos;
using User.Authentication.Application.Features.oAuth.Common.Exceptions;
using User.Authentication.Application.Features.oAuth.Common.Interfaces;
using User.Authentication.Domain.Features.oAuth.Models;

namespace User.Authentication.Infrastructure.Features.OAuth.Repositories;

internal class TokenRepository(IConfiguration configuration) : ITokenRepository
{
    public async Task<Token> GenerateToken(TokenRequestDto dto, CancellationToken cancellationToken)
        {
            var authOptions = configuration
                .GetSection("AuthenticationOptions")
                .Get<ServiceAuthenticationOptions>();

            using var httpClient = new HttpClient();

            var response = await httpClient.RequestPasswordTokenAsync(
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

        public async Task<Token> RefreshToken(TokenRequestDto dto, string refreshToken, CancellationToken cancellationToken)
        {
            var authOptions = configuration
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