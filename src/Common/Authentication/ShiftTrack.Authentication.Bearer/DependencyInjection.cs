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

                    // WebSockets can't send the Authorization header: SignalR passes the token in the query string.
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            if (!string.IsNullOrEmpty(accessToken)
                                && context.HttpContext.Request.Path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services
                .AddAuthorization(options =>
                {
                });

            return services;
        }
    }
}
