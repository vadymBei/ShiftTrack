using Microsoft.AspNetCore.Identity;
using ShiftTrack.Data.Interfaces;
using User.Authentication.Core.Application.Common.Dto;

namespace User.Authentication.Core.Application.Common.Interfaces
{
    public interface IRoleService : IEntityServiceBase<IdentityRole>
    {
        Task<IdentityRole> CreateRole(RoleToCreateDto createDto);

        Task<IList<IdentityRole>> GetRoles(CancellationToken cancellationToken);

        Task<IdentityRole> GetRoleByName(string roleName, CancellationToken cancellationToken);

        Task<bool> CheckRoleExist(string name);
    }
}
