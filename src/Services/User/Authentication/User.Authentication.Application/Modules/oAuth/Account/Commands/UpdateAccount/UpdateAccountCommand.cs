using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Account.Commands.UpdateAccount;

public record UpdateAccountCommand(
    string Id,
    string Email,
    string PhoneNumber) : IRequest<UserVm>;