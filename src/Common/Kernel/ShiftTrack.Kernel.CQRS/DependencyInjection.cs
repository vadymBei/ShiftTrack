using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel.Attributes;
using ShiftTrack.Kernel.CQRS.Behaviors;
using ShiftTrack.Kernel.CQRS.Implementations;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Kernel.CQRS;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(assembly => assembly.GetTypes().Any(t => t.IsDefined(typeof(ShiftTrackMemberAttribute))))
            .Distinct()
            .ToArray();

        // Register validators
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Combined registration for both types of handlers
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()

            // Register handlers without response
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()

            // Register handlers with response
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Register validation decorators
        services.TryDecorate(typeof(IRequestHandler<>), typeof(ValidationDecorator<>));
        services.TryDecorate(typeof(IRequestHandler<,>), typeof(ValidationDecorator<,>));
        
        // Register domain events dispatcher
        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
        return services;
    }
}