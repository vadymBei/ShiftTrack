using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel;
using ShiftTrack.Kernel.Attributes;
using ShiftTrack.Kernel.CQRS;
using User.Authentication.Application.Features.oAuth.Common.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.Services;
using User.Authentication.Domain.Features.oAuth.Options;

namespace User.Authentication.Application;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKernel();
            
        services.AddCqrs();
            
        services.Configure<AuthClientOptions>(configuration.GetSection("AuthClientOptions"));
        
        //Services
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserRoleService, UserRoleService>();
        services.AddTransient<IRoleService, RoleService>();
        
        return services;
    }
}