using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Kernel.Exceptions;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Exceptions;
using User.Authentication.Core.Application.Common.Interfaces;

namespace User.Authentication.Core.Application.Common.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(
        RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public Task<bool> CheckRoleExist(string name)
    {
        return _roleManager.Roles
            .AnyAsync(r => r.Name == name);
    }

    public async Task<IdentityRole> CreateRole(RoleToCreateDto createDto)
    {
        var roleExist = await _roleManager
            .RoleExistsAsync(createDto.Name);

        if (roleExist)
        {
            throw new RoleAlreadyExistException();
        }

        var identityRole = new IdentityRole(createDto.Name);

        var result = await _roleManager
            .CreateAsync(identityRole);

        if (!result.Succeeded)
        {
            throw new CreateRoleException(result.Errors?.FirstOrDefault()?.Description);
        }

        return identityRole;
    }

    public async Task<IdentityRole> GetById(object id, CancellationToken cancellationToken)
    {
        var role = await _roleManager.Roles
            .FirstOrDefaultAsync(x => x.Id == (string)id, cancellationToken);

        if (role == null)
        {
            throw new EntityNotFoundException(typeof(IdentityRole), (string)id);
        }

        return role;
    }

    public async Task<IdentityRole> GetRoleByName(string roleName, CancellationToken cancellationToken)
    {
        var role = await _roleManager.Roles
            .FirstOrDefaultAsync(x => x.Name == roleName, cancellationToken);

        return role;
    }

    public async Task<IList<IdentityRole>> GetRoles(CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles
            .ToListAsync(cancellationToken);

        return roles;
    }
}