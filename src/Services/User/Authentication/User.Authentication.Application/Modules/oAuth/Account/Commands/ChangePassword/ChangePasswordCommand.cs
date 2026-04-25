using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.Dtos;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Account.Commands.ChangePassword;

public record ChangePasswordCommand(
    ChangePasswordDto Data) : IRequest<TokenVm>;