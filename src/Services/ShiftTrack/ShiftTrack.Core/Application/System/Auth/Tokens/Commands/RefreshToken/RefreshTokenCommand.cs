using MediatR;
using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.Auth.Tokens.Commands.RefreshToken
{
    public record RefreshTokenCommand(
        RefreshTokenDto Data) : IRequest<TokenVM>;
}
