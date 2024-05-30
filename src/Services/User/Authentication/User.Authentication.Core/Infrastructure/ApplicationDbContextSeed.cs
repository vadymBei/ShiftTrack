using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Interfaces;

namespace User.Authentication.Core.Infrastructure
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(IUserService usersService)
        {
            var adminExist = await usersService.CheckUserExist("+380971234567");

            if (!adminExist)
            {
                await usersService.CreateUser(
                    new UserToCreateDto
                    (
                        "admin@shifttrack.api",
                        "+380971234567",
                        "VZI2N]V[sn$NvFf"
                    ));
            }
        }
    }
}
