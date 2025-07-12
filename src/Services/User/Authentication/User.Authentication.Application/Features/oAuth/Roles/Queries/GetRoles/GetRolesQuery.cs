using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Features.oAuth.Roles.Queries.GetRoles;

public record GetRolesQuery : IRequest<IEnumerable<RoleVm>>;