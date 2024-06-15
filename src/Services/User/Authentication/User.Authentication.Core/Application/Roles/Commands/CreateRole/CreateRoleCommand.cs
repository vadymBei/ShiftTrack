using MediatR;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Roles.Commands.CreateRole
{
    public record CreateRoleCommand(
        RoleToCreateDto Data) : IRequest<RoleVM>;
}
