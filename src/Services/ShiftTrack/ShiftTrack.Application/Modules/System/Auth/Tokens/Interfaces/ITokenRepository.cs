using ShiftTrack.Application.Modules.System.Auth.Tokens.Dtos;
using ShiftTrack.Domain.Modules.System.Auth.Models;

namespace ShiftTrack.Application.Modules.System.Auth.Tokens.Interfaces;

public interface ITokenRepository
{
    Task<Token> GenerateToken(GenerateTokenDto dto, CancellationToken cancellationToken);
    Task<Token> RefreshToken(RefreshTokenDto dto, CancellationToken cancellationToken);
}