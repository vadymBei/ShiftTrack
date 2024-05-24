using User.Authentication.Core.Domain.Models.OAuth;

namespace User.Authentication.Core.Application.Common.Interfaces
{
    public interface ITokenService
    {
        Task<Token> GenerateToken(string login, string password, CancellationToken cancellationToken);

        Task<Token> RefreshToken(string refreshToken, CancellationToken cancellationToken);
    }
}
