using Gateway.Web.Api.Common.Headers;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using MMLib.SwaggerForOcelot;          // Простір для SwaggerForOcelot
using Swashbuckle.AspNetCore.SwaggerUI; // Простір імен для Swagger UI


var builder = WebApplication.CreateBuilder(args);

// add default auth server authentication
var authenticationProviderKey = "IdentityApiKey";

builder.Services
    .AddAuthentication()
    .AddJwtBearer(authenticationProviderKey, options =>
    {
        options.Authority = "Authority";
        options.Audience = "Audience";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddCors();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services
    .AddOcelot(builder.Configuration)
    .AddPolly();

// Add Swagger documentation for standard Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

app.UseSwaggerForOcelotUI(x =>
{
    x.PathToSwaggerGenerator = "/swagger/docs";
    
    x.DownstreamSwaggerHeaders = new Dictionary<string, string>
    {
        {
            DefaultHeaders.Localization,
            AcceptLanguages.Ukrainian
        },
        {
            DefaultHeaders.Authorization,
            "Bearer "
        }
    };
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

app.UseAuthentication();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();

app.Run();