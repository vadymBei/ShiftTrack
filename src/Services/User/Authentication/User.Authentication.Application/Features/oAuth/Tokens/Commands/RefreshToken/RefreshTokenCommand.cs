using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Features.oAuth.Tokens.Commands.RefreshToken;

public record RefreshTokenCommand(
    string RefreshToken) : IRequest<TokenVm>;