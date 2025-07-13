using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.Dtos;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Features.oAuth.Account.Commands.ChangePassword;

public record ChangePasswordCommand(
    ChangePasswordDto Data) : IRequest<TokenVm>;