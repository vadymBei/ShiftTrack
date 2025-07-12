using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Features.oAuth.Tokens.Commands.GenerateToken;

public record GenerateTokenCommand(
    string Login, 
    string Password) : IRequest<TokenVm>;