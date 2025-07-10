using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Features.System.Auth.Common.Dtos;
using ShiftTrack.Application.Features.System.Auth.Common.Interfaces;
using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Domain.Features.System.Auth.Models;

namespace ShiftTrack.Infrastructure.Features.System.Auth.Repositories;

public class TokenRepository(
    IClient client,
    ILogger<TokenRepository> logger) : ITokenRepository
{
    public async Task<Token> GenerateToken(GenerateTokenDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var token = await client
                .Path("user-authentication-api/request-authentication-service")
                .Auth(AuthProvider.Basic)
                .Body(dto)
                .Post<Token>("tokens/generate", cancellationToken);

            return token;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while generating token");
            throw;
        }
    }

    public async Task<Token> RefreshToken(RefreshTokenDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var token = await client
                .Path("user-authentication-api/request-authentication-service")
                .Auth(AuthProvider.Basic)
                .Body(dto)
                .Post<Token>("tokens/refresh", cancellationToken);

            return token;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while refreshing token");
            throw;
        }
    }
}