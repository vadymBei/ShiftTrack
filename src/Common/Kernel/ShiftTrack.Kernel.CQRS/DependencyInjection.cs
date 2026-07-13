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
    /// <summary>
    /// Реєструє Mediator, handlers, validators та декоратори валідації.
    /// </summary>
    /// <param name="services">Колекція сервісів DI.</param>
    /// <param name="assemblies">
    /// Явно передайте збірки з handlers/validators.
    /// Якщо не вказано — шукає збірки з <see cref="MediatorMemberAttribute"/>.
    /// </param>
    public static IServiceCollection AddCqrs(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        // Mediator не має стану — Transient безпечний і швидший за Scoped
        services.AddTransient<IMediator, Mediator>();

        // Автоматично читає CancellationToken з HttpContext.RequestAborted
        services.AddHttpContextAccessor();
        services.AddTransient<ICancellationTokenProvider, HttpContextCancellationTokenProvider>();

        var targetAssemblies = assemblies.Length > 0
            ? assemblies
            : AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.GetTypes().Any(t => t.IsDefined(typeof(ShiftTrackMemberAttribute), false)))
                .ToArray();

        if (targetAssemblies.Length == 0)
            throw new InvalidOperationException(
                "No assemblies found. Either pass assemblies explicitly to AddCqrs() " +
                $"or mark a class with [{nameof(ShiftTrackMemberAttribute)}].");

        // Register validators
        services.Scan(scan => scan
            .FromAssemblies(targetAssemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Register handlers
        services.Scan(scan => scan
            .FromAssemblies(targetAssemblies)

            // INotificationHandler<> охоплює і IDomainEventHandler<> (через наслідування),
            // тому окремий рядок для IDomainEventHandler більше не потрібен
            .AddClasses(classes => classes.AssignableTo(typeof(INotificationHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Register validation decorators
        services.TryDecorate(typeof(IRequestHandler<>), typeof(ValidationDecorator<>));
        services.TryDecorate(typeof(IRequestHandler<,>), typeof(ValidationDecorator<,>));

        return services;
    }
}