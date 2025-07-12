using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Kernel.Exceptions;
using User.Authentication.Application.Features.oAuth.Common.Dtos;
using User.Authentication.Application.Features.oAuth.Common.Exceptions;
using User.Authentication.Application.Features.oAuth.Common.Interfaces;

namespace User.Authentication.Application.Features.oAuth.Common.Services;

public class RoleService(
    RoleManager<IdentityRole> roleManager) : IRoleService
{
    public Task<bool> CheckRoleExist(string name)
    {
        return roleManager.Roles
            .AnyAsync(r => r.Name == name);
    }

    public async Task<IdentityRole> CreateRole(RoleToCreateDto createDto)
    {
        var roleExist = await roleManager
            .RoleExistsAsync(createDto.Name);

        if (roleExist)
        {
            throw new RoleAlreadyExistException();
        }

        var identityRole = new IdentityRole(createDto.Name);

        var result = await roleManager
            .CreateAsync(identityRole);

        if (!result.Succeeded)
        {
            throw new CreateRoleException(result.Errors?.FirstOrDefault()?.Description);
        }

        return identityRole;
    }

    public async Task<IdentityRole> GetById(object id, CancellationToken cancellationToken)
    {
        var role = await roleManager.Roles
            .FirstOrDefaultAsync(x => x.Id == (string)id, cancellationToken);

        if (role == null)
        {
            throw new EntityNotFoundException(typeof(IdentityRole), (string)id);
        }

        return role;
    }

    public async Task<IdentityRole> GetRoleByName(string roleName, CancellationToken cancellationToken)
    {
        var role = await roleManager.Roles
            .FirstOrDefaultAsync(x => x.Name == roleName, cancellationToken);

        return role;
    }

    public async Task<IList<IdentityRole>> GetRoles(CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles
            .ToListAsync(cancellationToken);

        return roles;
    }
}