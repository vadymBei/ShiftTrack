using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Features.oAuth.Account.Commands.UpdateAccount;

public record UpdateAccountCommand(
    string Id,
    string Email,
    string PhoneNumber) : IRequest<UserVm>;