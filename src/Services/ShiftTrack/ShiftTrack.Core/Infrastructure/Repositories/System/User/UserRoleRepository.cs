using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.User;

public class UserRoleRepository(
    IClient client) : IUserRoleRepository
{
    public async Task CreateUserRole(UserRoleToCreateDto dto, CancellationToken cancellationToken)
    {
        await client
            .Path("user-authentication-api/request-authentication-service")
            .Auth(AuthProvider.Basic)
            .Body(dto)
            .Post("user-roles", cancellationToken);
    }
}