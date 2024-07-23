using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.Auth.Common.Interfaces;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.Auth.Tokens.Commands.GenerateToken
{
    public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, TokenVM>
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public GenerateTokenCommandHandler(
            IMapper mapper,
            ITokenService tokenService)
        {
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<TokenVM> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _tokenService
                .GenerateToken(request.Data, cancellationToken);

            return _mapper.Map<TokenVM>(token);
        }
    }
}
