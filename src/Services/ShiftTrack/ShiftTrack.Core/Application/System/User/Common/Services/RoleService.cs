using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.Roles.Models;

namespace ShiftTrack.Core.Application.System.User.Common.Services;

public class RoleService(
    IRoleRepository roleRepository) : IRoleService
{
    public Task<IEnumerable<Role>> GetRoles(CancellationToken cancellationToken)
    {
        return roleRepository.GetRoles(cancellationToken);
    }
}