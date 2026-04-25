using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.System.User.Common.Dtos;
using ShiftTrack.Application.Modules.System.User.Common.Interfaces;
using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Domain.Modules.System.Auth.Models;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.System.User.Repositories;

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
                .Post<Token>("account/change-password", cancellationToken);

            return token;
        }
        catch (HttpClientException ex)
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
                .Post<Authentication.Models.User>("account/register", cancellationToken);

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
                .Put<Authentication.Models.User>("account", cancellationToken);

            return user;
        }
        catch (HttpClientException ex)
        {
            logger.LogError($"Error occurred while updating user: {ex.InnerException?.Message}");
            throw;
        }
    }
}