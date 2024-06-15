using MediatR;
using User.Authentication.Core.Application.Common.Dto;

namespace User.Authentication.Core.Application.UserRoles.Commands.CreateUserRole
{
    public record CreateUserRoleCommand(
        UserRoleToCreateDto Data) : IRequest;
}
