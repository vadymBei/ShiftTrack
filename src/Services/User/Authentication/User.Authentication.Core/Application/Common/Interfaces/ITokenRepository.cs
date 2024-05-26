using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Domain.Models.OAuth;

namespace User.Authentication.Core.Application.Common.Interfaces
{
    public interface ITokenRepository
    {
        Task<Token> GenerateToken(TokenRequestDto dto, CancellationToken cancellationToken);

        Task<Token> RefreshToken(TokenRequestDto dto, string refreshToken, CancellationToken cancellationToken);
    }
}
