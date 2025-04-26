using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Services;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.Services;

namespace ShiftTrack.Core.Application.Organization;

public static class OrganizationModule
{
    public static IServiceCollection AddOrganizationServices(this IServiceCollection services)
    {
        //Services
        //Structure
        services.AddTransient<IUnitService, UnitService>();
        services.AddTransient<IDepartmentService, DepartmentService>();
        services.AddTransient<IPositionService, PositionService>();
        
        //Timesheet
        services.AddTransient<IShiftService, ShiftService>();

        //Repositories
        
        return services;
    }
}