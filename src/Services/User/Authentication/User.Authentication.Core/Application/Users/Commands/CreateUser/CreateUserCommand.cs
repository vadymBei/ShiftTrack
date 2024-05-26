using MediatR;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(
        long ProfileId, 
        string Email, 
        string FullName, 
        string Password) : IRequest<UserVM>;
}
