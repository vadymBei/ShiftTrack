using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.Dtos;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Features.oAuth.Roles.Commands.CreateRole;

public record CreateRoleCommand(
    RoleToCreateDto Data) : IRequest<RoleVm>;