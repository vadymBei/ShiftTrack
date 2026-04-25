using AutoMapper;
using ShiftTrack.Application.Modules.System.Auth.Common.Interfaces;
using ShiftTrack.Application.Modules.System.Auth.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Tokens.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IMapper mapper,
    ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, TokenVm>
{
    public async Task<TokenVm> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await tokenService
            .RefreshToken(request.Data, cancellationToken);

        return mapper.Map<TokenVm>(token);
    }
}