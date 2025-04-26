using ShiftTrack.Authentication.Basic;
using ShiftTrack.Authentication.Basic.Extensions;
using ShiftTrack.Authentication.Identity;
using ShiftTrack.Kernel.Extensions;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerUI;
using User.Authentication.Core;
using User.Authentication.Core.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreServices(builder.Configuration);

builder.Services.AddIdentityStorage<ApplicationDbContext>(builder.Configuration);

builder.Services.AddBasicAuthentication(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddBasicAuthenticationSwagger(
    builder.Environment.ApplicationName,
    Assembly.GetEntryAssembly().GetName().Version.ToString());

var app = builder.Build();

app.UseKernelExceptionHandler();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", app.Environment.ApplicationName);
    c.DocExpansion(DocExpansion.None);
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

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
