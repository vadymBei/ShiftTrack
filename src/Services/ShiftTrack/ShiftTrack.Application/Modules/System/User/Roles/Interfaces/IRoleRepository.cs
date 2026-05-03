using ShiftTrack.Domain.Modules.System.User.Roles.Entities;

namespace ShiftTrack.Application.Modules.System.User.Roles.Interfaces;

public interface IRoleRepository
{
    Task<Role> GetById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Role>> GetAll(CancellationToken cancellationToken);
    Task<Role> GetByName(string name, CancellationToken cancellationToken);
}