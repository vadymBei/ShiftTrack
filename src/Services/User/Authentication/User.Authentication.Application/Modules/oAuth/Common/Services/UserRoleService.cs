using Microsoft.AspNetCore.Identity;
using User.Authentication.Application.Modules.oAuth.Common.Dtos;
using User.Authentication.Application.Modules.oAuth.Common.Exceptions;
using User.Authentication.Application.Modules.oAuth.Common.Interfaces;
using EntityUser = ShiftTrack.Authentication.Models.User;

namespace User.Authentication.Application.Modules.oAuth.Common.Services;

public class UserRoleService(
    IUserService userService,
    IRoleService roleService,
    UserManager<EntityUser> userManager)
    : IUserRoleService
{
    public async Task CreateUserRole(UserRoleToCreateDto dto)
    {
        var user = await userService
            .GetById(dto.UserId, CancellationToken.None);

        var role = await roleService
            .GetById(dto.RoleId, CancellationToken.None);

        var result = await userManager
            .AddToRoleAsync(user, role.Name);

        if (!result.Succeeded)
        {
            throw new CreateUserRoleException(result.Errors?.FirstOrDefault()?.Description);
        }
    }
}