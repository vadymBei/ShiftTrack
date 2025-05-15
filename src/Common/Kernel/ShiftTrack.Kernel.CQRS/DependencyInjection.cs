using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel.Attributes;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Kernel.CQRS;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        
        var shiftTrackAssemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(assembly => assembly.GetTypes().Any(t => t.IsDefined(typeof(ShiftTrackMemberAttribute))))
            .ToArray();
        
        services.Scan(scan => scan
            .FromAssemblies(shiftTrackAssemblies)
        
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}