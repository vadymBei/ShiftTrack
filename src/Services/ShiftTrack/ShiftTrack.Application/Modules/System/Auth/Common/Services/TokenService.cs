using ShiftTrack.Application.Modules.System.Auth.Common.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Common.Interfaces;
using ShiftTrack.Domain.Modules.System.Auth.Models;

namespace ShiftTrack.Application.Modules.System.Auth.Common.Services;

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