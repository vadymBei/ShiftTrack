using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Tokens.Commands.GenerateToken;

public class GenerateTokenCommandHandler(
    IMapper mapper,
    ITokenService tokenService) : IRequestHandler<GenerateTokenCommand, TokenVm>
{
    public async Task<TokenVm> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await tokenService
            .GenerateToken(request.Login, request.Password, cancellationToken);

        return mapper.Map<TokenVm>(token);
    }
}