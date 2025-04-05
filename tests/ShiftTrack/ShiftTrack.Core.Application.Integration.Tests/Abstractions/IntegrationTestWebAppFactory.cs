using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Core.Infrastructure;
using Testcontainers.PostgreSql;

namespace ShiftTrack.Core.Application.Integration.Tests.Abstractions;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:14")
        .WithDatabase("shift-track-dev")
        .WithUsername("postgres")
        .WithPassword("123258")
        .Build();

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseNpgsql(
                        _dbContainer.GetConnectionString(),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });
        });
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}