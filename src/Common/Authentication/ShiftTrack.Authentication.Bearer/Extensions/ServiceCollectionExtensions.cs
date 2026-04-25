using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace ShiftTrack.Authentication.Bearer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJWTAuthenticationSwagger(this IServiceCollection services, string title, string version)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = title,
                    Version = version
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", document),
                        new List<string>()
                    }
                });
            });

            return services;
        }
    }
}
