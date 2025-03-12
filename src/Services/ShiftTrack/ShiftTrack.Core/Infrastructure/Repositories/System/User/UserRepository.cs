using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.Tokens.Models;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.User;

public class UserRepository : IUserRepository
{
    private readonly IClient _client;

    public UserRepository(
        IClient client)
    {
        _client = client;
    }

    public async Task<Token> ChangePassword(ChangeUserPasswordDto dto, CancellationToken cancellationToken)
    {
        var token = await _client
            .Path("user-authentication-api/request-authentication-service")
            .Auth(AuthProvider.Basic)
            .Body(dto)
            .Post<Token>("users/change-password", cancellationToken);

        return token;
    }

    public async Task<Authentication.Models.User> RegisterUser(UserToRegisterDto dto, CancellationToken cancellationToken)
    {
        var user = await _client
            .Path("user-authentication-api/request-authentication-service")
            .Auth(AuthProvider.Basic)
            .Body(dto)
            .Post<Authentication.Models.User>("users/register", cancellationToken);

        return user;
    }

    public async Task<Authentication.Models.User> UpdateUser(UserToUpdateDto dto, CancellationToken cancellationToken)
    {
        var user = await _client
            .Path("user-authentication-api/request-authentication-service")
            .Auth(AuthProvider.Basic)
            .Body(dto)
            .Put<Authentication.Models.User>("users", cancellationToken);

        return user;
    }
}