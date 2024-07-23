using ShiftTrack.Core.Domain.System.User.Roles.Models;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetRoles(CancellationToken cancellationToken);
    }
}
