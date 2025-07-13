using Microsoft.AspNetCore.Identity;
using ShiftTrack.Data.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.Dtos;

namespace User.Authentication.Application.Features.oAuth.Common.Interfaces;

public interface IRoleService : IEntityServiceBase<IdentityRole>
{
    Task<IdentityRole> CreateRole(RoleToCreateDto createDto);
    Task<IList<IdentityRole>> GetRoles(CancellationToken cancellationToken);
    Task<IdentityRole> GetRoleByName(string roleName, CancellationToken cancellationToken);
    Task<bool> CheckRoleExist(string name);
}