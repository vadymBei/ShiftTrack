using Gateway.Web.Api.Common.Headers;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

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
        },
    };
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