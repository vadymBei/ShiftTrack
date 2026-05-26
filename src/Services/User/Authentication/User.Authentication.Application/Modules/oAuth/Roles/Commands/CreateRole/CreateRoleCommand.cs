using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.Dtos;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Roles.Commands.CreateRole;

public record CreateRoleCommand(
    RoleToCreateDto Data) : IRequest<RoleVm>;