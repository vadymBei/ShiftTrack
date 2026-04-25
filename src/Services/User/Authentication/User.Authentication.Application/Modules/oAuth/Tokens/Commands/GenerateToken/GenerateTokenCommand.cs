using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Tokens.Commands.GenerateToken;

public record GenerateTokenCommand(
    string Login, 
    string Password) : IRequest<TokenVm>;