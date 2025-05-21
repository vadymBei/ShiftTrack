using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Tokens.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IMapper mapper,
    ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, TokenVM>
{
    public async Task<TokenVM> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await tokenService
            .RefreshToken(request.RefreshToken, cancellationToken);

        return mapper.Map<TokenVM>(token);
    }
}