using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Features.oAuth.Account.Commands.Register;

public record RegisterCommand(
    string Email,
    string PhoneNumber,
    string Password) : IRequest<UserVm>;