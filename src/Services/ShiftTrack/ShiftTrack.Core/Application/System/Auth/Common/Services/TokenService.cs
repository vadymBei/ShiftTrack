using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.Interfaces;
using ShiftTrack.Core.Domain.System.Tokens.Models;

namespace ShiftTrack.Core.Application.System.Auth.Common.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;

        public TokenService(
            ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public Task<Token> GenerateToken(GenerateTokenDto dto, CancellationToken cancellationToken)
        {
            return _tokenRepository.GenerateToken(dto, cancellationToken);
        }

        public Task<Token> RefreshToken(RefreshTokenDto dto, CancellationToken cancellationToken)
        {
            return _tokenRepository.RefreshToken(dto, cancellationToken);
        }
    }
}
