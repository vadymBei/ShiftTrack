using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.WebClient.Http.Extensions;
using ShiftTrack.WebClient.Http.Interfaces;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.User.Employees
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IWebClient _webClient;

        public AuthenticationRepository(
            IWebClient webClient)
        {
            _webClient = webClient;
        }

        public async Task<Authentication.Models.User> RegisterUser(UserToRegisterDto dto, CancellationToken cancellationToken)
        {
            var user = await _webClient
                .BasicAuthentication("user-authentication-api/request-users")
                .Path("user-authentication-api/request-users")
                .Body(dto)
                .Post<Authentication.Models.User>("register", cancellationToken);

            return user;
        }

        public async Task<Authentication.Models.User> UpdateUser(UserToUpdateDto dto, CancellationToken cancellationToken)
        {
            var user = await _webClient
                .BasicAuthentication("user-authentication-api/request-users")
                .Path("user-authentication-api/request-users")
                .Body(dto)
                .Put<Authentication.Models.User>(null, cancellationToken);

            return user;
        }
    }
}
