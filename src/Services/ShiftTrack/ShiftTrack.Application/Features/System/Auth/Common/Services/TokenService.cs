using ShiftTrack.Application.Features.System.Auth.Common.Dtos;
using ShiftTrack.Application.Features.System.Auth.Common.Interfaces;
using ShiftTrack.Domain.Features.System.Auth.Models;

namespace ShiftTrack.Application.Features.System.Auth.Common.Services;

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