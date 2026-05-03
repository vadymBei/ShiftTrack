using ShiftTrack.Application.Modules.System.Auth.Tokens.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Tokens.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Tokens.UseCases.Commands.RefreshToken;

public record RefreshTokenCommand(
    RefreshTokenDto Data) : IRequest<TokenVm>;