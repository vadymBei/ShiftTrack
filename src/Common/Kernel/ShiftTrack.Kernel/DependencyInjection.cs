using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel.Attributes;

namespace ShiftTrack.Kernel;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddKernel(this IServiceCollection services)
    {
        var shiftTrackAssemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(assembly => assembly.GetTypes().Any(t => t.IsDefined(typeof(ShiftTrackMemberAttribute))))
            .ToArray();

        // register common components
        services.AddAutoMapper(shiftTrackAssemblies);
        
        services.AddValidatorsFromAssemblies(shiftTrackAssemblies);
        
        return services;
    }
}