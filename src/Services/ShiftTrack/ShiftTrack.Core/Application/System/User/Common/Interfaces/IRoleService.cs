using ShiftTrack.Core.Domain.System.User.Roles.Entities;
using ShiftTrack.Data.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces;

public interface IRoleService : IEntityServiceBase<Role>
{
    Task<IEnumerable<Role>> GetRoles(CancellationToken cancellationToken);
}