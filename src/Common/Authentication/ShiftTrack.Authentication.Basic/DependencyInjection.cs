using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Authentication.Basic.Handlers;
using ShiftTrack.Authentication.Options;

namespace ShiftTrack.Authentication.Basic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBasicAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceAuthenticationOptions>(configuration.GetSection("AuthenticationOptions"));

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            return services;
        }
    }
}
