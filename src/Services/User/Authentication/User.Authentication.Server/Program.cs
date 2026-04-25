using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Authentication.Application;
using User.Authentication.Infrastructure;
using User.Authentication.Infrastructure.Modules.OAuth.ApiResources;
using User.Authentication.Infrastructure.Modules.OAuth.ApiScopes;
using User.Authentication.Infrastructure.Modules.OAuth.Clients;
using User.Authentication.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

// Disable Static Web Assets in Docker to avoid DirectoryNotFoundException for NuGet packages
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    builder.WebHost.UseStaticWebAssets(); 
}

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddIdentity<ShiftTrack.Authentication.Models.User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddIdentityServer(options =>
    {
        options.IssuerUri = "http://user.authentication.server:8080";
    })
    .AddAspNetIdentity<ShiftTrack.Authentication.Models.User>()
    .AddOperationalStore<ApplicationDbContext>()
    .AddInMemoryClients([ShiftTrackClient.Register()])
    .AddInMemoryApiResources([ShiftTrackApi.Register()])
    .AddInMemoryApiScopes(ShiftTrackApiScopes.Get())
    .AddInMemoryIdentityResources([
        new IdentityResources.Email(),
        new IdentityResources.Phone(),
        new IdentityResource("role", ["role"]),
        new IdentityResource("id", ["id"])
    ]);

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        dataContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[DEBUG_LOG] Database migration failed. Error: {ex.Message}");
        if (!app.Environment.IsDevelopment())
        {
            throw;
        }
        Console.WriteLine("[DEBUG_LOG] Continuing execution because we are in Development mode.");
    }
}

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.Run();