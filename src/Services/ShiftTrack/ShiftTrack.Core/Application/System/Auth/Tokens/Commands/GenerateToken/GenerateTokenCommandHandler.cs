using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.Auth.Common.Interfaces;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.Auth.Tokens.Commands.GenerateToken;

public class GenerateTokenCommandHandler(
    IMapper mapper,
    ITokenService tokenService) : IRequestHandler<GenerateTokenCommand, TokenVM>
{
    public async Task<TokenVM> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await tokenService
            .GenerateToken(request.Data, cancellationToken);

        return mapper.Map<TokenVM>(token);
    }
}