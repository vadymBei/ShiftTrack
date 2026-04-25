using ShiftTrack.Application.Modules.System.Auth.Common.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Tokens.Commands.RefreshToken;

public record RefreshTokenCommand(
    RefreshTokenDto Data) : IRequest<TokenVm>;