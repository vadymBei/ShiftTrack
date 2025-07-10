using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.System.Auth.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Client.Http;
using ShiftTrack.Infrastructure.Features.System.Auth.Repositories;
using ShiftTrack.Infrastructure.Features.System.User.Repositories;
using ShiftTrack.Infrastructure.Persistence;
using ShiftTrack.Infrastructure.Services;
using ShiftTrack.Kernel.Attributes;

namespace ShiftTrack.Infrastructure;

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

        //Services
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        //Repositories
        //System
        //Auth
        services.AddTransient<ITokenRepository, TokenRepository>();
        
        //User
        services.AddTransient<IUserRepository, UserRepository>();
        
        return services;
    }
}