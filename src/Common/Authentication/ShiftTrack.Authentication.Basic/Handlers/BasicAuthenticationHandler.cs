using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShiftTrack.Authentication.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ShiftTrack.Authentication.Basic.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ServiceAuthenticationOptions _serviceAuthenticationOptions;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            IOptions<ServiceAuthenticationOptions> serviceAuthenticationOptions)
                : base(options, loggerFactory, urlEncoder)
        {
            _serviceAuthenticationOptions = serviceAuthenticationOptions.Value;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            ServiceUser user;

            await Task.Delay(0);

            var endpoint = Context.GetEndpoint();

            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return AuthenticateResult.NoResult();
            }

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);

                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);

                var userName = credentials[0];
                var password = credentials[1];

                user = GetServiceUser(userName, password);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Invalid Authorization Header: {ex.Message}");
            }

            if (user == null)
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            var claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Login),
                new Claim(ClaimTypes.Name, user.Login)
            };

            foreach (var role in user.Roles)
            {
                claimsList.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = claimsList.ToArray();

            var identity = new ClaimsIdentity(claims, Scheme.Name);

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private ServiceUser GetServiceUser(string userName, string password)
        {
            var user = _serviceAuthenticationOptions.Users
                .FirstOrDefault(c => c.Login == userName && c.Password == password);

            return user;
        }
    }
}
