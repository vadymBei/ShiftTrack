using Generators.Pdf.Application.Common.Interfaces;
using Generators.Pdf.Application.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel;
using ShiftTrack.Kernel.Attributes;
using ShiftTrack.Kernel.CQRS;

namespace Generators.Pdf.Application;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddKernel();

        services.AddCqrs();

        services.AddTransient<IPdfGeneratorService, PdfGeneratorService>();

        return services;
    }
}