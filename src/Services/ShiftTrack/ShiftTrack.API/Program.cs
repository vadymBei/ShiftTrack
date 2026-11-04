using ShiftTrack.Infrastructure.Modules.Communication.Messenger.Services;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application;
using ShiftTrack.Authentication.Bearer;
using ShiftTrack.Authentication.Bearer.Extensions;
using ShiftTrack.Infrastructure;
using ShiftTrack.Infrastructure.Persistence;
using ShiftTrack.Infrastructure.Realtime;
using ShiftTrack.Kernel.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddJWTAuthentication(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddRealtime(builder.Configuration);

// The files of the chats are on this instance's disk, so they are cleaned up here.
builder.Services.AddHostedService<AttachmentCleanupService>();

builder.Services.AddJWTAuthenticationSwagger(
    builder.Environment.ApplicationName,
    Assembly.GetEntryAssembly()?.GetName().Version?.ToString());

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    applicationDbContext.Database.Migrate();

    await ApplicationDbContextSeed.SeedAsync(applicationDbContext);
}

app.UseSwagger();

app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", app.Environment.ApplicationName);
    x.DocExpansion(DocExpansion.None);
});

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html", permanent: false);
        return;
    }

    await next();
});

app.UseHttpsRedirection();

app.UseKernelExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationsHub>(NotificationsHub.Path);

// Chat files lie in wwwroot but are only for the members of the conversation:
// they are served by the attachment endpoint, never as static files.
app.UseWhen(
    context => context.Request.Path.StartsWithSegments(LocalAttachmentStorage.RequestPath, StringComparison.OrdinalIgnoreCase),
    branch => branch.Run(context =>
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return Task.CompletedTask;
    }));

app.UseStaticFiles();

app.Run();

public partial class Program
{
}

