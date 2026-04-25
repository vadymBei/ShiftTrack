using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Roles.Queries.GetRoles;

public record GetRolesQuery : IRequest<IEnumerable<RoleVm>>;