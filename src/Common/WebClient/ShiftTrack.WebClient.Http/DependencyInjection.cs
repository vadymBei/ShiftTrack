using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.WebClient.Http.Configuration;
using ShiftTrack.WebClient.Http.Interfaces;
using ShiftTrack.WebClient.Http.Options;

namespace ShiftTrack.WebClient.Http
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddClientHttp(this IServiceCollection services, IConfiguration configuration)
        {
            // register options
            services.Configure<HttpClientOptions>(configuration.GetSection("HttpClient"));

            // add context accessor
            services.AddHttpContextAccessor();

            // register IHttpClientFactory
            services.AddHttpClient("default");

            // register web client services
            services.AddTransient<ClientConfiguration>();
            services.AddTransient<IWebClient, WebClient>();

            return services;
        }
    }
}
