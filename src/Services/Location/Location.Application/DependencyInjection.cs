using Location.Application.Common.Interfaces;
using Location.Application.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel;
using ShiftTrack.Kernel.Attributes;
using ShiftTrack.Kernel.CQRS;

namespace Location.Application;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddKernel();

        services.AddCqrs();

        // Services
        services.AddTransient<ILocationService, LocationService>();
        
        return services;
    }
}