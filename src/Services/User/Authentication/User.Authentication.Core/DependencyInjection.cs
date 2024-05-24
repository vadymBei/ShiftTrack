using Kernel;
using Kernel.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.Services;
using User.Authentication.Core.Domain.Options;
using User.Authentication.Core.Infrastructure;
using User.Authentication.Core.Infrastructure.Repositories;

namespace User.Authentication.Core
{
    [ShiftTrackMember]
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddKernel();

            services.Configure<AuthClientOptions>(configuration.GetSection("AuthClientOptions"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<ITokenRepository, TokenRepository>();

            return services;
        }
    }
}
