using AutoMapper;
using MediatR;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Tokens.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenVM>
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(
            IMapper mapper,
            ITokenService tokenService)
        {
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<TokenVM> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _tokenService
                .RefreshToken(request.RefreshToken, cancellationToken);

            return _mapper.Map<TokenVM>(token);
        }
    }
}
