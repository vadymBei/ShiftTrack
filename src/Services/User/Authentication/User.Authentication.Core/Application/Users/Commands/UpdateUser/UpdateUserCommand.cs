using MediatR;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(
        string Id,
        string Email,
        string PhoneNumber) : IRequest<UserVM>;
}
