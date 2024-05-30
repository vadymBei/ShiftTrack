using AutoMapper;
using MediatR;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Tokens.Commands.GenerateToken
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
                .GenerateToken(request.PhoneNumber, request.Password, cancellationToken);

            return _mapper.Map<TokenVM>(token);
        }
    }
}
