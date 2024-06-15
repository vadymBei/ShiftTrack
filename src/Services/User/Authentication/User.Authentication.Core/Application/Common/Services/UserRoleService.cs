using Microsoft.AspNetCore.Identity;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Exceptions;
using User.Authentication.Core.Application.Common.Interfaces;

using EntityUser = ShiftTrack.Authentication.Models.User;

namespace User.Authentication.Core.Application.Common.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly UserManager<EntityUser> _userManager;

        public UserRoleService(
            IUserService userService,
            IRoleService roleService,
            UserManager<EntityUser> userManager)
        {
            _userService = userService;
            _roleService = roleService;
            _userManager = userManager;
        }

        public async Task CreateUserRole(UserRoleToCreateDto dto)
        {
            var user = await _userService
                .GetById(dto.UserId, CancellationToken.None);

            var role = await _roleService
                .GetById(dto.RoleId, CancellationToken.None);

            var result = await _userManager
                .AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                throw new CreateUserRoleException(result.Errors?.FirstOrDefault()?.Description);
            }
        }
    }
}
