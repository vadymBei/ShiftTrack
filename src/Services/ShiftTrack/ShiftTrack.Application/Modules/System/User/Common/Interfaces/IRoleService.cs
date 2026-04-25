using ShiftTrack.Data.Interfaces;
using ShiftTrack.Domain.Modules.System.User.Roles.Entities;

namespace ShiftTrack.Application.Modules.System.User.Common.Interfaces;

public interface IRoleService : IEntityServiceBase<Role>
{
    Task<IEnumerable<Role>> GetRoles(CancellationToken cancellationToken);
}