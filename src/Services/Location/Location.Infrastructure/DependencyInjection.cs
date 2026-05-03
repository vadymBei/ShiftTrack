using Location.Application.Common.Interfaces;
using Location.Infrastructure.Persistence;
using Location.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Client.Http;
using ShiftTrack.Kernel.Attributes;

namespace Location.Infrastructure;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddClientHttp(configuration);
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Repositories
        services.AddTransient<INominatimRepository, NominatimRepository>();
        services.AddTransient<ILocationRepository, LocationRepository>();
        
        return services;
    }
}