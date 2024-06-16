using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.Roles.Models;
using ShiftTrack.WebClient.Http.Extensions;
using ShiftTrack.WebClient.Http.Interfaces;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.User.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IWebClient _webClient;

        public RoleRepository(IWebClient webClient)
        {
            _webClient = webClient;
        }

        public async Task<IEnumerable<Role>> GetRoles(CancellationToken cancellationToken)
        {
            var roles = await _webClient
                .BasicAuthentication("user-authentication-api/request-authentication-service")
                .Path("user-authentication-api/request-authentication-service")
                .Get<IEnumerable<Role>>("roles", cancellationToken);

            return roles;
        }
    }
}
