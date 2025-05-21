using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Kernel.Exceptions;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Exceptions;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Domain.Models.OAuth;
using EntityUser = ShiftTrack.Authentication.Models.User;

namespace User.Authentication.Core.Application.Common.Services;

public class UserService(
    ITokenService tokenService,
    UserManager<EntityUser> userManager)
    : IUserService
{
    public async Task<Token> ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken)
    {
        var user = await GetById(dto.UserId, cancellationToken);

        var result = await userManager
            .ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

        if (!result.Succeeded)
        {
            throw new ChangePasswordException(result.Errors?.FirstOrDefault()?.Description);
        }

        var token = await tokenService
            .GenerateToken(user.PhoneNumber, dto.NewPassword, cancellationToken);

        return token;
    }

    public async Task<IEnumerable<EntityUser>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await userManager.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return users;

    }

    public async Task<bool> CheckUserExist(string phoneNumber)
    {
        return await userManager.Users
            .AnyAsync(x => x.PhoneNumber == phoneNumber);
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

        var result = await userManager
            .CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            throw new CreateUserException(result.Errors?.FirstOrDefault()?.Description);
        }

        return user;
    }

    public async Task<EntityUser> GetById(object id, CancellationToken cancellationToken)
    {
        var user = await userManager
            .FindByIdAsync((string)id);

        if (user == null)
        {
            throw new EntityNotFoundException(typeof(EntityUser), (string)id);
        }

        return user;
    }

    public async Task<EntityUser> UpdateUser(UserToUpdateDto dto)
    {
        var user = await userManager
            .FindByIdAsync(dto.Id);

        if (user == null)
        {
            throw new EntityNotFoundException(typeof(EntityUser), dto.Id);
        }

        if (user.UserName != dto.PhoneNumber)
        {
            user.UserName = dto.PhoneNumber;
            user.PhoneNumber = dto.PhoneNumber;
        }

        user.Email = dto.Email;

        var result = await userManager
            .UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new UpdateUserException(result.Errors?.FirstOrDefault()?.Description);
        }

        return await userManager
            .FindByIdAsync(dto.Id);
    }
}