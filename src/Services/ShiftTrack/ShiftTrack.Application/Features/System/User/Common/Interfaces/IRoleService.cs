using ShiftTrack.Data.Interfaces;
using ShiftTrack.Domain.Features.System.User.Roles.Entities;

namespace ShiftTrack.Application.Features.System.User.Common.Interfaces;

public interface IRoleService : IEntityServiceBase<Role>
{
    Task<IEnumerable<Role>> GetRoles(CancellationToken cancellationToken);
}