using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.Interfaces;
using ShiftTrack.Core.Domain.System.Tokens.Models;

namespace ShiftTrack.Core.Application.System.Auth.Common.Services;

public class TokenService(
    ITokenRepository tokenRepository) : ITokenService
{
    public Task<Token> GenerateToken(GenerateTokenDto dto, CancellationToken cancellationToken)
    {
        return tokenRepository.GenerateToken(dto, cancellationToken);
    }

    public Task<Token> RefreshToken(RefreshTokenDto dto, CancellationToken cancellationToken)
    {
        return tokenRepository.RefreshToken(dto, cancellationToken);
    }
}