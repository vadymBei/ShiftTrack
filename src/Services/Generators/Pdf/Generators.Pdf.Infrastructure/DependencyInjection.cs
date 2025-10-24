using Generators.Pdf.Application.Common.Interfaces;
using Generators.Pdf.Infrastructure.Features.PuppeteerSharp.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel.Attributes;

namespace Generators.Pdf.Infrastructure;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IPuppeteerSharpRepository, PuppeteerSharpRepository>();
        
        return services;
    }
}