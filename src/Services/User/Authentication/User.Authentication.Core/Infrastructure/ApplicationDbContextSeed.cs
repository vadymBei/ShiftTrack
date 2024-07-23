using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Interfaces;

namespace User.Authentication.Core.Infrastructure
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultRolesAsync(
            IRoleService rolesService)
        {
            var sysAdminRoleExist = await rolesService
               .CheckRoleExist("SysAdmin");

            if (!sysAdminRoleExist)
            {
                await rolesService.CreateRole(
                    new RoleToCreateDto(
                        "SysAdmin"));
            }

            var stylistRoleExist = await rolesService
                   .CheckRoleExist("Stylist");

            if (!stylistRoleExist)
            {
                await rolesService.CreateRole(
                    new RoleToCreateDto(
                        "Stylist"));
            }

            var regionalDirectorExist = await rolesService
                .CheckRoleExist("RegionalDirector");

            if (!regionalDirectorExist)
            {
                await rolesService.CreateRole(
                    new RoleToCreateDto(
                        "RegionalDirector"));
            }

            var departmentDirectorExist = await rolesService
                .CheckRoleExist("DepartmentDirector");

            if (!departmentDirectorExist)
            {
                await rolesService.CreateRole(
                    new RoleToCreateDto(
                        "DepartmentDirector"));
            }
        }

        public static async Task SeedDefaultUserAsync(
            IUserService usersService,
            IRoleService rolesService,
            IUserRoleService userRoleService)
        {
            var sysAdminRole = await rolesService
                .GetRoleByName("SysAdmin", CancellationToken.None);

            var adminExist = await usersService
                .CheckUserExist("+380971234567");

            if (!adminExist)
            {
                var user = await usersService.CreateUser(
                    new UserToCreateDto
                    (
                        "admin@shifttrack.api",
                        "+380971234567",
                        "VZI2N]V[sn$NvFf"
                    ));

                if (sysAdminRole is not null)
                {
                    await userRoleService.CreateUserRole(
                        new UserRoleToCreateDto(
                            user.Id,
                            sysAdminRole.Id));
                }
            }
        }
    }
}
