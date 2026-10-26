using ShiftTrack.Kernel.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Common.Behaviours;
using ShiftTrack.Application.Common.Outbox.Interfaces;
using ShiftTrack.Application.Common.Outbox.Services;
using ShiftTrack.Application.Modules.Booking;
using ShiftTrack.Application.Modules.Communication;
using ShiftTrack.Application.Modules.Organization;
using ShiftTrack.Application.Modules.System;
using ShiftTrack.Kernel;
using ShiftTrack.Kernel.CQRS;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKernel();

        services.AddCqrs();
        
        // Register Pipelines
        services.AddTransient(typeof(IPipelineBehaviour<,>), typeof(RequestAuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehaviour<>), typeof(RequestAuthorizationBehaviour<>));
        services.AddTransient(typeof(IPipelineBehaviour<,>), typeof(TransactionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehaviour<>), typeof(TransactionBehaviour<>));

        // Outbox
        services.AddTransient<IDomainEventOutbox, DomainEventOutbox>();
        services.AddTransient<IOutboxProcessor, OutboxProcessor>();

        services.AddBookingServices();
        services.AddOrganizationServices();
        services.AddSystemServices();
        services.AddCommunicationServices();

        return services;
    }
}