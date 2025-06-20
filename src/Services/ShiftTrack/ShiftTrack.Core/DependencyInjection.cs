﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Authentication.Extensions;
using ShiftTrack.Client.Http;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization;
using ShiftTrack.Core.Application.System;
using ShiftTrack.Core.Infrastructure.Persistence;
using ShiftTrack.Kernel;
using ShiftTrack.Kernel.Attributes;
using ShiftTrack.Kernel.CQRS;

namespace ShiftTrack.Core;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKernel();

        services.AddCqrs();

        services.AddClientHttp(configuration);

        services.AddCurrentUserService();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

        services.AddOrganizationServices();
        services.AddSystemServices();

        return services;
    }
}