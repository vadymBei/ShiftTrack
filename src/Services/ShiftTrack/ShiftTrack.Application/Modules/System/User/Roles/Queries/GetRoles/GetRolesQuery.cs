using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.Roles.Queries.GetRoles;

public record GetRolesQuery() : IRequest<IEnumerable<RoleVm>>;