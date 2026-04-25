using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.Dtos;

namespace User.Authentication.Application.Modules.oAuth.UserRoles.Commands.CreateUserRole;

public record CreateUserRoleCommand(
    UserRoleToCreateDto Data) : IRequest;