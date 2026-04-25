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
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
