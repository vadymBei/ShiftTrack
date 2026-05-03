using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Services;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Services;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Services;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Services;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShiftHistories.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShiftHistories.Services;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Services;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Services;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Services;

namespace ShiftTrack.Application.Modules.Organization;

public static class OrganizationModule
{
    public static IServiceCollection AddOrganizationServices(this IServiceCollection services)
    {
        //Employees
        services.AddTransient<IEmployeeService, EmployeeService>();

        //Structure
        services.AddTransient<IUnitService, UnitService>();
        services.AddTransient<IPositionService, PositionService>();
        services.AddTransient<IDepartmentService, DepartmentService>();

        //Timesheet
        services.AddTransient<IEmployeeShiftService, EmployeeShiftService>();
        services.AddTransient<IEmployeeShiftHistoryService, EmployeeShiftHistoryService>();
        services.AddTransient<IShiftService, ShiftService>();
        services.AddTransient<IUnitTimesheetService, UnitTimesheetService>();

        return services;
    }
}