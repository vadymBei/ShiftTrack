using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Domain.System.Tokens.Models;

namespace ShiftTrack.Core.Application.System.Auth.Common.Interfaces
{
    public interface ITokenService
    {
        Task<Token> GenerateToken(GenerateTokenDto dto, CancellationToken cancellationToken);

        Task<Token> RefreshToken(RefreshTokenDto dto, CancellationToken cancellationToken);
    }
}
