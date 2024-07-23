using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.WebClient.Http.Extensions;
using ShiftTrack.WebClient.Http.Interfaces;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.User
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IWebClient _webClient;

        public UserRoleRepository(
            IWebClient webClient)
        {
            _webClient = webClient;
        }

        public async Task CreateUserRole(UserRoleToCreateDto dto, CancellationToken cancellationToken)
        {
            await _webClient
                .BasicAuthentication("user-authentication-api/request-authentication-service")
                .Path("user-authentication-api/request-authentication-service")
                .Body(dto)
                .Post("user-roles", cancellationToken);
        }
    }
}
