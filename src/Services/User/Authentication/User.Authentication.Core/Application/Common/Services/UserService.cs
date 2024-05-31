using Kernel.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Exceptions;
using User.Authentication.Core.Application.Common.Interfaces;

using EntityUser = ShiftTrack.Authentication.Models.User;

namespace User.Authentication.Core.Application.Common.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<EntityUser> _userManager;

        public UserService(
            UserManager<EntityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CheckUserExist(string phoneNumber)
        {
            return await _userManager.Users.AnyAsync(x => x.PhoneNumber == phoneNumber);
        }

        public async Task<EntityUser> CreateUser(UserToCreateDto dto)
        {
            var userExist = await CheckUserExist(dto.PhoneNumber);

            if (userExist)
            {
                throw new UserAlreadyExistException(dto.PhoneNumber);
            }

            var user = new EntityUser()
            {
                UserName = dto.PhoneNumber,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            var result = await _userManager
                .CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                throw new CreateUserException(result.Errors?.FirstOrDefault()?.Description);
            }

            return user;
        }

        public async Task<EntityUser> UpdateUser(UserToUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(EntityUser), dto.Id);
            }

            user.UserName = dto.PhoneNumber;
            user.PhoneNumber = dto.PhoneNumber;
            user.Email = dto.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new UpdateUserException(result.Errors?.FirstOrDefault()?.Description);
            }

            return await _userManager.FindByIdAsync(dto.Id);
        }
    }
}
