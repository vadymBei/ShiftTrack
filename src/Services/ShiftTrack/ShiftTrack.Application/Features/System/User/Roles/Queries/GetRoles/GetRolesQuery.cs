using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.Roles.Queries.GetRoles;

public record GetRolesQuery() : IRequest<IEnumerable<RoleVm>>;