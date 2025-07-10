using AutoMapper;
using ShiftTrack.Application.Features.System.Auth.Common.Interfaces;
using ShiftTrack.Application.Features.System.Auth.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Tokens.Commands.GenerateToken;

public class GenerateTokenCommandHandler(
    IMapper mapper,
    ITokenService tokenService) : IRequestHandler<GenerateTokenCommand, TokenVm>
{
    public async Task<TokenVm> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await tokenService
            .GenerateToken(request.Data, cancellationToken);

        return mapper.Map<TokenVm>(token);
    }
}