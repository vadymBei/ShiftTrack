using ShiftTrack.Application.Modules.System.Auth.Common.Dtos;
using ShiftTrack.Domain.Modules.System.Auth.Models;

namespace ShiftTrack.Application.Modules.System.Auth.Common.Interfaces;

public interface ITokenService
{
    Task<Token> GenerateToken(GenerateTokenDto dto, CancellationToken cancellationToken);
    Task<Token> RefreshToken(RefreshTokenDto dto, CancellationToken cancellationToken);
}