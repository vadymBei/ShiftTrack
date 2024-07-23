using MediatR;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.Roles.Queries.GetRoles
{
    public class GetRolesQuery : IRequest<IEnumerable<RoleVM>>
    {
    }
}
