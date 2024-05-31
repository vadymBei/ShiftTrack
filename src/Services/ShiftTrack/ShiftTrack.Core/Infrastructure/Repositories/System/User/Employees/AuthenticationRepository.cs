using Data.WebClient.Enums;
using Data.WebClient.Extensions;
using Data.WebClient.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;

namespace ShiftTrack.Core.Infrastructure.Repositories.System.User.Employees
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IWebClient _webClient;

        public AuthenticationRepository(
            IWebClient webClient)
        {
            _webClient = webClient;

            _webClient.Configure(c =>
            {
                c.WebResourcePath = "user-authentication-api/request-users";
                c.ErrorHandlingMode = ErrorHandlingMode.Auto;
                c.IgnoreSslErrors = false;
                c.AuthenticationType = AuthenticationType.Basic;
            });
        }

        public async Task<Authentication.Models.User> RegisterUser(UserToRegisterDto dto, CancellationToken cancellationToken)
        {
            var user = await _webClient
                .WithStringContent(dto)
                .Post<Authentication.Models.User>("register", cancellationToken);

            return user;
        }

        public async Task<Authentication.Models.User> UpdateUser(UserToUpdateDto dto, CancellationToken cancellationToken)
        {
            var user = await _webClient
                .WithStringContent(dto)
                .Put<Authentication.Models.User>(null, cancellationToken);

            return user;
        }
    }
}
