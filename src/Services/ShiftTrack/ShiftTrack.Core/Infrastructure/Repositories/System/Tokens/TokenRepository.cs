using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.Interfaces;
using ShiftTrack.Core.Domain.System.Tokens.Models;
using Microsoft.Extensions.Logging;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.Tokens;

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