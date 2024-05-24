using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Interfaces;

namespace User.Authentication.Core.Infrastructure
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(IUserService usersService)
        {
            var adminExist = await usersService.CheckUserExist("admin@shifttrack.api");

            if (!adminExist)
            {
                await usersService.CreateUser(
                    new UserToCreateDto
                    (
                        "admin@shifttrack.api",
                        "Admin",
                        "VZI2N]V[sn$NvFf",
                        1
                    ));
            }
        }
    }
}
