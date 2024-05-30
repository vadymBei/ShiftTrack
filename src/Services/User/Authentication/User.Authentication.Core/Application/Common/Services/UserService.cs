using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Exceptions;
using User.Authentication.Core.Application.Common.Interfaces;

using IdentityUser = ShiftTrack.Authentication.Models.User;

namespace User.Authentication.Core.Application.Common.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<bool> CheckUserExist(string phoneNumber)
        {
            return _userManager.Users.AnyAsync(x => x.PhoneNumber == phoneNumber);
        }

        public async Task<IdentityUser> CreateUser(UserToCreateDto dto)
        {
            var user = new IdentityUser()
            {
                UserName = dto.PhoneNumber,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                throw new CreateUserException(result.Errors?.FirstOrDefault()?.Description);
            }

            return user;
        }
    }
}
