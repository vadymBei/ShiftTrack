using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

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

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "BasicAuthentication"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
