using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace ShiftTrack.Authentication.Basic.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBasicAuthenticationSwagger(this IServiceCollection services, string title, string version)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = title,
                        Version = version
                    });

                x.AddSecurityDefinition(
                    "BasicAuthentication",
                    new OpenApiSecurityScheme()
                    {
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Scheme = "BasicAuthentication",
                        Type = SecuritySchemeType.ApiKey
                    });

                x.AddSecurityRequirement(document => new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecuritySchemeReference("BasicAuthentication", document),
                        new List<string>()
                    }
                });
            });

            return services;
        }
    }
}
