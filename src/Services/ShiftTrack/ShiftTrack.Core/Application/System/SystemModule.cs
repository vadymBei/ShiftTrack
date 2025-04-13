using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Core.Application.System.Auth.Common.Interfaces;
using ShiftTrack.Core.Application.System.Auth.Common.Services;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Services;
using ShiftTrack.Core.Application.System.User.Common.Strategies;
using ShiftTrack.Core.Infrastructure.Repositories.System.Tokens;
using ShiftTrack.Core.Infrastructure.Repositories.System.User;

namespace ShiftTrack.Core.Application.System;

public static class SystemModule
{
    public static IServiceCollection AddSystemServices(this IServiceCollection services)
    {
        //Services
        //Auth
        services.AddTransient<ITokenService, TokenService>();

        //User
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IEmployeeService, EmployeeService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IEmployeeRoleService, EmployeeRoleService>();
        services.AddTransient<IEmployeeRoleChecker, EmployeeRoleChecker>();
        
        //Strategies
        services.AddTransient<IEmployeeRoleStrategyFactory, EmployeeRoleStrategyFactory>();
        services.AddTransient<IEmployeeRoleStrategy, EmployeeRoleSysAdminStrategy>();
        services.AddTransient<IEmployeeRoleStrategy, EmployeeRoleUnitDirectorStrategy>();
        services.AddTransient<IEmployeeRoleStrategy, EmployeeRoleDepartmentDirectorStrategy>();
        
        
        //Repositories
        //Auth
        services.AddTransient<ITokenRepository, TokenRepository>();
        
        //User
        services.AddTransient<IUserRepository, UserRepository>();
        
        return services;
    }
}