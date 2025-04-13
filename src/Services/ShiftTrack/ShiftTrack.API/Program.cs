using Microsoft.EntityFrameworkCore;
using ShiftTrack.Authentication.Bearer;
using ShiftTrack.Authentication.Bearer.Extensions;
using ShiftTrack.Core;
using ShiftTrack.Core.Infrastructure;
using System.Reflection;
using System.Text.Json.Serialization;
using ShiftTrack.Core.Infrastructure.Persistence;
using ShiftTrack.Kernel.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreServices(builder.Configuration);

builder.Services.AddJWTAuthentication(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

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

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseKernelExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}

