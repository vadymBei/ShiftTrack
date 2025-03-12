using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.Interfaces;
using ShiftTrack.Core.Domain.System.Tokens.Models;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.Tokens;

public class TokenRepository : ITokenRepository
{
    private readonly IClient _client;

    public TokenRepository(
        IClient client)
    {
        _client = client;
    }

    public async Task<Token> GenerateToken(GenerateTokenDto dto, CancellationToken cancellationToken)
    {
        var token = await _client
            .Path("user-authentication-api/request-authentication-service")
            .Auth(AuthProvider.Basic)
            .Body(dto)
            .Post<Token>("tokens/generate", cancellationToken);

        return token;
    }

    public async Task<Token> RefreshToken(RefreshTokenDto dto, CancellationToken cancellationToken)
    {
        var token = await _client
            .Path("user-authentication-api/request-authentication-service")
            .Auth(AuthProvider.Basic)
            .Body(dto)
            .Post<Token>("tokens/refresh", cancellationToken);

        return token;
    }
}