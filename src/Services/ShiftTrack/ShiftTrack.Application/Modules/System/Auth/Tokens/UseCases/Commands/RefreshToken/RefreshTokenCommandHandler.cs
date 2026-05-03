using AutoMapper;
using ShiftTrack.Application.Modules.System.Auth.Tokens.Interfaces;
using ShiftTrack.Application.Modules.System.Auth.Tokens.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Tokens.UseCases.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IMapper mapper,
    ITokenRepository tokenRepository) : IRequestHandler<RefreshTokenCommand, TokenVm>
{
    public async Task<TokenVm> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await tokenRepository
            .RefreshToken(request.Data, cancellationToken);

        return mapper.Map<TokenVm>(token);
    }
}