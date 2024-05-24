using Kernel.Extensions;
using ShiftTrack.Authentication.Bearer;
using ShiftTrack.Authentication.Bearer.Extensions;
using ShiftTrack.Authentication.Identity;
using System.Reflection;
using User.Authentication.Core;
using User.Authentication.Core.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreServices(builder.Configuration);

builder.Services.AddIdentityStorage<ApplicationDbContext>(builder.Configuration);

builder.Services.AddJWTAuthentication(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddJWTAuthenticationSwagger(
    Assembly.GetEntryAssembly().GetName().ToString(),
    Assembly.GetEntryAssembly().GetName().Version.ToString());

var app = builder.Build();

app.UseKernelExceptionHandler();

app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", app.Environment.ApplicationName));

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
