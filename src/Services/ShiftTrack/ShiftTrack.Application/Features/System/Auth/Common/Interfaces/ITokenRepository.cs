using ShiftTrack.Application.Features.System.Auth.Common.Dtos;
using ShiftTrack.Domain.Features.System.Auth.Models;

namespace ShiftTrack.Application.Features.System.Auth.Common.Interfaces;

public interface ITokenRepository
{
    Task<Token> GenerateToken(GenerateTokenDto dto, CancellationToken cancellationToken);
    Task<Token> RefreshToken(RefreshTokenDto dto, CancellationToken cancellationToken);
}