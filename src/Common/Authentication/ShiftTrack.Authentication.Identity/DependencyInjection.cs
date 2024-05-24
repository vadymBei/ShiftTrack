using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Authentication.Models;

namespace ShiftTrack.Authentication.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityStorage<TContext>(this IServiceCollection services, IConfiguration configuration)
          where TContext : DbContext
        {
            // add identity storage
            services
                .AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TContext>();

            return services;
        }
    }
}
