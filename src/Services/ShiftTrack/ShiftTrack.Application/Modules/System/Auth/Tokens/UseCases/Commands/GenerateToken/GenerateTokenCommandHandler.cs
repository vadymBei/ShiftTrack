using AutoMapper;
using ShiftTrack.Application.Modules.System.Auth.Tokens.Interfaces;
using ShiftTrack.Application.Modules.System.Auth.Tokens.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Tokens.UseCases.Commands.GenerateToken;

public class GenerateTokenCommandHandler(
    IMapper mapper,
    ITokenRepository tokenRepository) : IRequestHandler<GenerateTokenCommand, TokenVm>
{
    public async Task<TokenVm> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await tokenRepository.GenerateToken(request.Data, cancellationToken);

        return mapper.Map<TokenVm>(token);
    }
}