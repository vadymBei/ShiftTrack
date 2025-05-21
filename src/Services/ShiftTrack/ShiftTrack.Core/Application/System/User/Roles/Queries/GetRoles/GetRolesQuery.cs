using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Roles.Queries.GetRoles;

public class GetRolesQuery : IRequest<IEnumerable<RoleVM>>
{
}