using ShiftTrack.Application.Modules.System.User.Roles.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.Roles.UseCases.Queries.GetRoles;

public record GetRolesQuery() : IRequest<IEnumerable<RoleVm>>;