using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Tokens.Commands.RefreshToken;

public record RefreshTokenCommand(
    string RefreshToken) : IRequest<TokenVm>;