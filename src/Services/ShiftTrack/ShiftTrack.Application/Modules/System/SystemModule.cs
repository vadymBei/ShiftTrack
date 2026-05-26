using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Services;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Strategies;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Services;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Strategies;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Services;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Strategies;

namespace ShiftTrack.Application.Modules.System;

public static class SystemModule
{
    public static IServiceCollection AddSystemServices(this IServiceCollection services)
    {
        //Services
        services.AddTransient<IEmployeeRoleChecker, EmployeeRoleChecker>();
        services.AddTransient<IEmployeeRoleService, EmployeeRoleService>();
        services.AddTransient<IEmployeeRoleUnitService, EmployeeRoleUnitService>();
        services.AddTransient<IEmployeeRoleUnitDepartmentService, EmployeeRoleUnitDepartmentService>();
        
        //Strategies
        services.AddTransient<IEmployeeRoleStrategyFactory, EmployeeRoleStrategyFactory>();
        services.AddTransient<IEmployeeRoleStrategy, SysAdminEmployeeRoleStrategy>();
        services.AddTransient<IEmployeeRoleStrategy, UnitDirectorEmployeeRoleStrategy>();
        services.AddTransient<IEmployeeRoleStrategy, DepartmentDirectorEmployeeRoleStrategy>();
        
        services.AddTransient<IEmployeeRoleUnitStrategyFactory, EmployeeRoleUnitStrategyFactory>();
        services.AddTransient<IEmployeeRoleUnitStrategy, DepartmentDirectorEmployeeRoleUnitStrategy>();
        services.AddTransient<IEmployeeRoleUnitStrategy, SysAdminEmployeeRoleUnitStrategy>();
        services.AddTransient<IEmployeeRoleUnitStrategy, UnitDirectorEmployeeRoleUnitStrategy>();
        
        services.AddTransient<IEmployeeRoleUnitDepartmentStrategyFactory, EmployeeRoleUnitDepartmentStrategyFactory>();
        services.AddTransient<IEmployeeRoleUnitDepartmentStrategy, DepartmentDirectorEmployeeRoleUnitDepartmentStrategy>();
        services.AddTransient<IEmployeeRoleUnitDepartmentStrategy, SysAdminEmployeeRoleUnitDepartmentStrategy>();
        services.AddTransient<IEmployeeRoleUnitDepartmentStrategy, UnitDirectorEmployeeRoleUnitDepartmentStrategy>();
        
        return services;
    }
}