using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Account.Commands.Register;

public record RegisterCommand(
    string Email,
    string PhoneNumber,
    string Password) : IRequest<UserVm>;