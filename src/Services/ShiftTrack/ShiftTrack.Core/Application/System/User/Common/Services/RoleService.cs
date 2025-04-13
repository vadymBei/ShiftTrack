using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.Roles.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Services;

public class RoleService(
    IApplicationDbContext applicationDbContext) : IRoleService
{
    public async Task<IEnumerable<Role>> GetRoles(CancellationToken cancellationToken)
    {
        var roles = await applicationDbContext.Roles
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .ToListAsync(cancellationToken);
        
        return roles;
    }
}