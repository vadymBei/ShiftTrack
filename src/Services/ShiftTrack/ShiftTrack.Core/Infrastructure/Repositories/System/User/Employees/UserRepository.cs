using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.Tokens.Models;
using ShiftTrack.WebClient.Http.Extensions;
using ShiftTrack.WebClient.Http.Interfaces;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.User.Employees
{
    public class UserRepository : IUserRepository
    {
        private readonly IWebClient _webClient;

        public UserRepository(
            IWebClient webClient)
        {
            _webClient = webClient;
        }

        public async Task<Token> ChangePassword(ChangeUserPasswordDto dto, CancellationToken cancellationToken)
        {
            var token = await _webClient
                .BasicAuthentication("user-authentication-api/request-authentication-service")
                .Path("user-authentication-api/request-authentication-service")
                .Body(dto)
                .Post<Token>("users/change-password", cancellationToken);

            return token;
        }

        public async Task<Authentication.Models.User> RegisterUser(UserToRegisterDto dto, CancellationToken cancellationToken)
        {
            var user = await _webClient
                .BasicAuthentication("user-authentication-api/request-authentication-service")
                .Path("user-authentication-api/request-authentication-service")
                .Body(dto)
                .Post<Authentication.Models.User>("users/register", cancellationToken);

            return user;
        }

        public async Task<Authentication.Models.User> UpdateUser(UserToUpdateDto dto, CancellationToken cancellationToken)
        {
            var user = await _webClient
                .BasicAuthentication("user-authentication-api/request-authentication-service")
                .Path("user-authentication-api/request-authentication-service")
                .Body(dto)
                .Put<Authentication.Models.User>("users", cancellationToken);

            return user;
        }
    }
}
