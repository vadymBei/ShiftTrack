using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Features.System.Auth.Common.Interfaces;
using ShiftTrack.Application.Features.System.Auth.Common.Services;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.Services;
using ShiftTrack.Application.Features.System.User.Common.Strategies;

namespace ShiftTrack.Application.Features.System;

public static class SystemModule
{
    public static IServiceCollection AddSystemServices(this IServiceCollection services)
    {
        //Auth
        services.AddTransient<ITokenService, TokenService>();
        
        //User
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IEmployeeRoleService, EmployeeRoleService>();
        services.AddTransient<IEmployeeRoleChecker, EmployeeRoleChecker>();
        
        //Strategies
        services.AddTransient<IEmployeeRoleStrategyFactory, EmployeeRoleStrategyFactory>();
        services.AddTransient<IEmployeeRoleStrategy, EmployeeRoleSysAdminStrategy>();
        services.AddTransient<IEmployeeRoleStrategy, EmployeeRoleUnitDirectorStrategy>();
        services.AddTransient<IEmployeeRoleStrategy, EmployeeRoleDepartmentDirectorStrategy>();
        
        return services;
    }
}