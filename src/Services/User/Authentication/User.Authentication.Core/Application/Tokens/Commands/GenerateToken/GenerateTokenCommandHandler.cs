using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Tokens.Commands.GenerateToken;

public class GenerateTokenCommandHandler(
    IMapper mapper,
    ITokenService tokenService) : IRequestHandler<GenerateTokenCommand, TokenVM>
{
    public async Task<TokenVM> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await tokenService
            .GenerateToken(request.Login, request.Password, cancellationToken);

        return mapper.Map<TokenVM>(token);
    }
}