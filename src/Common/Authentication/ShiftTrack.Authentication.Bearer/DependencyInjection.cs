using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Authentication.Options;

namespace ShiftTrack.Authentication.Bearer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authOptions = configuration
                .GetSection("AuthenticationOptions")
                .Get<ServiceAuthenticationOptions>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = authOptions.AuthServer.Authority;
                    options.Audience = authOptions.AuthServer.Audience;
                    options.RequireHttpsMetadata = authOptions.AuthServer.RequireHttpsMetadata;
                });

            services
                .AddAuthorization(options =>
                {
                });

            return services;
        }
    }
}
