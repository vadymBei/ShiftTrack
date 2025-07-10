using ShiftTrack.Application.Features.System.Auth.Common.Dtos;
using ShiftTrack.Application.Features.System.Auth.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Tokens.Commands.RefreshToken;

public record RefreshTokenCommand(
    RefreshTokenDto Data) : IRequest<TokenVm>;