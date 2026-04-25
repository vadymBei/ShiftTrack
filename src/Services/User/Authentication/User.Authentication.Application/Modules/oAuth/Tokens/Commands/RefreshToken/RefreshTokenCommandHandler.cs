using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Tokens.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IMapper mapper,
    ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, TokenVm>
{
    public async Task<TokenVm> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await tokenService
            .RefreshToken(request.RefreshToken, cancellationToken);

        return mapper.Map<TokenVm>(token);
    }
}