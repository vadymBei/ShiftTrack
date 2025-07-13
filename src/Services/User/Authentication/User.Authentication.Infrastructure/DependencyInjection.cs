using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel;
using ShiftTrack.Kernel.Attributes;
using ShiftTrack.Kernel.CQRS;
using User.Authentication.Application.Common.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.Interfaces;
using User.Authentication.Infrastructure.Features.OAuth.Repositories;
using User.Authentication.Infrastructure.Persistence;

namespace User.Authentication.Infrastructure;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKernel();
            
        services.AddCqrs();
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

        //Repositories
        services.AddTransient<ITokenRepository, TokenRepository>();

        return services;
    }
}