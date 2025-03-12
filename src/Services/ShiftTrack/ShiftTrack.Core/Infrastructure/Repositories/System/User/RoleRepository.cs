using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.Roles.Models;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.User;

public class RoleRepository : IRoleRepository
{
    private readonly IClient _client;

    public RoleRepository(IClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<Role>> GetRoles(CancellationToken cancellationToken)
    {
        var roles = await _client
            .Path("user-authentication-api/request-authentication-service")
            .Auth(AuthProvider.Basic)
            .Get<IEnumerable<Role>>("roles", cancellationToken);

        return roles;
    }
}