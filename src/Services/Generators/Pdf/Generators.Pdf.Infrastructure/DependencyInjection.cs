using Generators.Pdf.Application.Common.Interfaces;
using Generators.Pdf.Infrastructure.Features.PuppeteerSharp;
using Generators.Pdf.Infrastructure.Features.PuppeteerSharp.Interfaces;
using Generators.Pdf.Infrastructure.Features.PuppeteerSharp.Repositories;
using Generators.Pdf.Infrastructure.Features.PuppeteerSharp.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel.Attributes;

namespace Generators.Pdf.Infrastructure;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PuppeteerOptions>(configuration.GetSection("Puppeteer"));
        
        services.AddSingleton<IBrowserManager, BrowserManager>();
        services.AddTransient<IPuppeteerSharpRepository, PuppeteerSharpRepository>();
        
        return services;
    }
}