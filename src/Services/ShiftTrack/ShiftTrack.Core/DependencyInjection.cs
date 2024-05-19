using Kernel;
using Kernel.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Services;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.Services;
using ShiftTrack.Core.Infrastructure;

namespace ShiftTrack.Core
{
    [ShiftTrackMember]
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddKernel();

            services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseNpgsql(
                       configuration.GetConnectionString("DefaultConnection"),
                       b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            //Organization structure
            services.AddTransient<IUnitService, UnitService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IPositionService, PositionService>();

            //Organization timesheet
            services.AddTransient<IShiftService, ShiftService>();

            return services;
        }
    }
}
