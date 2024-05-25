using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Authentication.Interfaces;
using ShiftTrack.Authentication.Services;

namespace ShiftTrack.Authentication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCurrentUserService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
