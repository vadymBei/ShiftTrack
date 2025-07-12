using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.Dtos;

namespace User.Authentication.Application.Features.oAuth.UserRoles.Commands.CreateUserRole;

public record CreateUserRoleCommand(
    UserRoleToCreateDto Data) : IRequest;