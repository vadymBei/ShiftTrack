using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.Services;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.Services;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Services;

namespace ShiftTrack.Application.Features.Organization;

public static class OrganizationModule
{
    public static IServiceCollection AddOrganizationServices(this IServiceCollection services)
    {
        //Employees
        services.AddTransient<IEmployeeService, EmployeeService>();

        //Structure
        services.AddTransient<IUnitService, UnitService>();
        services.AddTransient<IDepartmentService, DepartmentService>();
        services.AddTransient<IPositionService, PositionService>();

        //Timesheet
        services.AddTransient<IShiftService, ShiftService>();
        services.AddTransient<ITimesheetService, TimesheetService>();

        return services;
    }
}