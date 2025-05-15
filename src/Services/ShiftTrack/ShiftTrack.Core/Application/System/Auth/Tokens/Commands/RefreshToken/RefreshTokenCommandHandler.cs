using AutoMapper;
using ShiftTrack.Core.Application.System.Auth.Common.Interfaces;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.Auth.Tokens.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IMapper mapper,
    ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, TokenVM>
{
    public async Task<TokenVM> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await tokenService
            .RefreshToken(request.Data, cancellationToken);

        return mapper.Map<TokenVM>(token);
    }
}