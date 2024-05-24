using Data.WebClient.Interfaces;
using Data.WebClient.Options;
using Data.WebClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.WebClient
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WebResourcesOptions>(configuration.GetSection("WebResources"));

            services.AddHttpContextAccessor();

            services.AddHttpClient();

            services.AddHttpClient("no-ssl-validation")
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler
                    {
                        ClientCertificateOptions = ClientCertificateOption.Manual,
                        ServerCertificateCustomValidationCallback =
                            (httpRequestMessage, cert, certChain, policyErrors) => true
                    };
                });

            services.AddTransient<IWebClient, WebClientEngine>();

            return services;
        }
    }
}
