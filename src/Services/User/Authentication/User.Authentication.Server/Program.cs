using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Authentication.Core;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Infrastructure;
using User.Authentication.Core.Infrastructure.OAuth.ApiResources;
using User.Authentication.Core.Infrastructure.OAuth.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreServices(builder.Configuration);

builder.Services
    .AddDefaultIdentity<ShiftTrack.Authentication.Models.User>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddIdentityServer()
    .AddApiAuthorization<ShiftTrack.Authentication.Models.User, ApplicationDbContext>(options =>
    {
        options.Clients.AddRange(ShiftTrackClient.Register());

        options.ApiResources.AddRange(ShiftTrackApi.Register());

        options.IdentityResources.AddEmail();

        options.IdentityResources.AddPhone();
        
        options.IdentityResources.Add(new IdentityResource("role", new[] { "role" }));

        options.IdentityResources.Add(new IdentityResource("id", new[] { "id" }));
    });

builder.Services
    .AddAuthentication()
    .AddIdentityServerJwt();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dataContext.Database.Migrate();

    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

    await ApplicationDbContextSeed.SeedDefaultUserAsync(userService);
}

app.UseAuthentication();

app.UseIdentityServer();

app.UseAuthorization();

app.Run();
