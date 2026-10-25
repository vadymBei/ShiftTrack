using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.API.Hubs;
using ShiftTrack.Application;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Authentication.Bearer;
using ShiftTrack.Authentication.Bearer.Extensions;
using ShiftTrack.Infrastructure;
using ShiftTrack.Infrastructure.Persistence;
using ShiftTrack.Kernel.Extensions;
using StackExchange.Redis;
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

var signalR = builder.Services
    .AddSignalR()
    .AddJsonProtocol(options =>
        options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var redisConnectionString = builder.Configuration.GetConnectionString("Redis");

if (!string.IsNullOrWhiteSpace(redisConnectionString))
{
    signalR.AddStackExchangeRedis(redisConnectionString, options =>
        options.Configuration.ChannelPrefix = RedisChannel.Literal("shift-track"));
}

builder.Services.AddScoped<INotificationPublisher, SignalRNotificationPublisher>();

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

app.UseStaticFiles();

app.Run();

public partial class Program
{
}

