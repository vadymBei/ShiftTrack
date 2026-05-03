using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.System.User.Roles.Interfaces;
using ShiftTrack.Domain.Modules.System.User.Roles.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.System.User.Roles.Repositories;

public class RoleRepository(
    IApplicationDbContext applicationDbContext) : IRoleRepository
{
    public async Task<Role> GetById(long id, CancellationToken cancellationToken)
    {
        var role = await applicationDbContext.Roles
                       .AsNoTracking()
                       .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                   ?? throw new EntityNotFoundException(typeof(Role), id);

        return role;
    }

    public async Task<IEnumerable<Role>> GetAll(CancellationToken cancellationToken)
    {
        var roles = await applicationDbContext.Roles
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .ToListAsync(cancellationToken);

        return roles;
    }

    public async Task<Role> GetByName(string name, CancellationToken cancellationToken)
    {
        var role = await applicationDbContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        return role;
    }
}