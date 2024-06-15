using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel.Attributes;
using ShiftTrack.Kernel.Behaviours;
using System.Reflection;

namespace ShiftTrack.Kernel
{
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

            services.AddMediatR(shiftTrackAssemblies);

            services.AddValidatorsFromAssemblies(shiftTrackAssemblies);

            // register behaviours
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}
