using ShiftTrack.Kernel.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Common.Behaviours;
using ShiftTrack.Application.Features.Booking;
using ShiftTrack.Application.Features.Organization;
using ShiftTrack.Application.Features.System;
using ShiftTrack.Client.Http;
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

        services.AddBookingServices();
        services.AddOrganizationServices();
        services.AddSystemServices();

        return services;
    }
}