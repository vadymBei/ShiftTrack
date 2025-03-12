using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.User;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly IClient _client;

    public UserRoleRepository(
        IClient client)
    {
        _client = client;
    }

    public async Task CreateUserRole(UserRoleToCreateDto dto, CancellationToken cancellationToken)
    {
        await _client
            .Path("user-authentication-api/request-authentication-service")
            .Auth(AuthProvider.Basic)
            .Body(dto)
            .Post("user-roles", cancellationToken);
    }
}