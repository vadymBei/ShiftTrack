using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Modules.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Common.Services;
using ShiftTrack.Application.Modules.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Common.Services;
using ShiftTrack.Application.Modules.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.Common.Services;

namespace ShiftTrack.Application.Modules.Organization;

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
        services.AddTransient<IEmployeeShiftService, EmployeeShiftService>();
        services.AddTransient<IEmployeeShiftHistoryService, EmployeeShiftHistoryService>();
        services.AddTransient<IShiftService, ShiftService>();
        services.AddTransient<ITimesheetService, TimesheetService>();

        return services;
    }
}