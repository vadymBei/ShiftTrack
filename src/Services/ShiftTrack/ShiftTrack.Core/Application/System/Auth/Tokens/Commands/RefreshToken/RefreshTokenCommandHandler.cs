using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.Auth.Common.Interfaces;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.Auth.Tokens.Commands.RefreshToken
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
                .RefreshToken(request.Data, cancellationToken);

            return _mapper.Map<TokenVM>(token);
        }
    }
}
