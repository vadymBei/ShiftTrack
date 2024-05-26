using Microsoft.Extensions.Options;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Domain.Models.OAuth;
using User.Authentication.Core.Domain.Options;

namespace User.Authentication.Core.Application.Common.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly AuthClientOptions _authClientOptions;

        public TokenService(
            ITokenRepository tokenRepository,
            IOptions<AuthClientOptions> authClientOptions)
        {
            _tokenRepository = tokenRepository;
            _authClientOptions = authClientOptions.Value;
        }

        public Task<Token> GenerateToken(string login, string password, CancellationToken cancellationToken)
        {
            return _tokenRepository.GenerateToken(
                new TokenRequestDto()
                {
                    Client = _authClientOptions.Client,
                    Scope = _authClientOptions.Scope,
                    ClientSecret = _authClientOptions.ClientSecret,
                    Login = login,
                    Password = password
                },
                cancellationToken);
        }

        public Task<Token> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            return _tokenRepository.RefreshToken(
                new TokenRequestDto()
                {
                    Client = _authClientOptions.Client,
                    ClientSecret = _authClientOptions.ClientSecret
                },
                refreshToken,
                cancellationToken);
        }
    }
}
