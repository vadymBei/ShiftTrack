
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Client.Http.Configs;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Client.Http.Options;

namespace ShiftTrack.Client.Http;

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
        services.AddTransient<ClientConfig>();
        services.AddTransient<IClient, Client>();

        return services;
    }
}