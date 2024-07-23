using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.Interfaces;
using ShiftTrack.Core.Domain.System.Tokens.Models;
using ShiftTrack.WebClient.Http.Extensions;
using ShiftTrack.WebClient.Http.Interfaces;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.Tokens
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IWebClient _webClient;

        public TokenRepository(
            IWebClient webClient)
        {
            _webClient = webClient;
        }

        public async Task<Token> GenerateToken(GenerateTokenDto dto, CancellationToken cancellationToken)
        {
           var token = await _webClient
               .BasicAuthentication("user-authentication-api/request-authentication-service")
               .Path("user-authentication-api/request-authentication-service")
               .Body(dto)
               .Post<Token>("tokens/generate", cancellationToken);

            return token;
        }

        public async Task<Token> RefreshToken(RefreshTokenDto dto, CancellationToken cancellationToken)
        {
            var token = await _webClient
                .BasicAuthentication("user-authentication-api/request-authentication-service")
                .Path("user-authentication-api/request-authentication-service")
                .Body(dto)
                .Post<Token>("tokens/refresh", cancellationToken);

            return token;
        }
    }
}
