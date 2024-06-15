using Microsoft.AspNetCore.Identity;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Interfaces;

namespace User.Authentication.Core.Infrastructure
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(
            IUserService usersService, 
            IRoleService rolesService,
            IUserRoleService userRoleService)
        {
            var adminRoleExist = await rolesService.CheckRoleExist("Admin");

            IdentityRole adminRole = null;

            if (!adminRoleExist)
            {
                adminRole = await rolesService.CreateRole(
                    new RoleToCreateDto(
                        "Admin"));
            }

            var adminExist = await usersService.CheckUserExist("+380971234567");

            if (!adminExist)
            {
                var user = await usersService.CreateUser(
                    new UserToCreateDto
                    (
                        "admin@shifttrack.api",
                        "+380971234567",
                        "VZI2N]V[sn$NvFf"
                    ));

                if (adminRole is not null)
                {
                    await userRoleService.CreateUserRole(
                        new UserRoleToCreateDto(
                            user.Id,
                            adminRole.Id));
                }
            }
        }
    }
}
