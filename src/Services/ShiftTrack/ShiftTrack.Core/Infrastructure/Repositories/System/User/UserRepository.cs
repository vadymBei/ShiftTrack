using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.Tokens.Models;
using Microsoft.Extensions.Logging;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.User;

public class UserRepository(
    IClient client,
    ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<Token> ChangePassword(ChangeUserPasswordDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var token = await client
                .Path("user-authentication-api/request-authentication-service")
                .Auth(AuthProvider.Basic)
                .Body(dto)
                .Post<Token>("users/change-password", cancellationToken);

            return token;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while changing password");
            throw;
        }
    }

    public async Task<Authentication.Models.User> RegisterUser(UserToRegisterDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var user = await client
                .Path("user-authentication-api/request-authentication-service")
                .Auth(AuthProvider.Basic)
                .Body(dto)
                .Post<Authentication.Models.User>("users/register", cancellationToken);

            return user;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while registering user");
            throw;
        }
    }

    public async Task<Authentication.Models.User> UpdateUser(UserToUpdateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var user = await client
                .Path("user-authentication-api/request-authentication-service")
                .Auth(AuthProvider.Basic)
                .Body(dto)
                .Put<Authentication.Models.User>("users", cancellationToken);

            return user;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error occurred while updating user: {ex.InnerException?.Message}");
            throw;
        }
    }
}