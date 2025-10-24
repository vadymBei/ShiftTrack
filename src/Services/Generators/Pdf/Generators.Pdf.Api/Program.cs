using System.Reflection;
using Generators.Pdf.Application;
using Generators.Pdf.Infrastructure;
using ShiftTrack.Authentication.Basic;
using ShiftTrack.Authentication.Basic.Extensions;
using ShiftTrack.Kernel.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
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