using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Email,
    string PhoneNumber,
    string Password) : IRequest<UserVM>;