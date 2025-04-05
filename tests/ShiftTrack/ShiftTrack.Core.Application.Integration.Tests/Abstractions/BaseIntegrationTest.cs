using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Core.Application.Data.Common.Interfaces;

namespace ShiftTrack.Core.Application.Integration.Tests.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _serviceScope;
    protected readonly ISender Sender;
    protected readonly IApplicationDbContext DbContext;

    protected BaseIntegrationTest(
        IntegrationTestWebAppFactory factory)
    {
        _serviceScope = factory.Services.CreateScope();

        Sender = _serviceScope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _serviceScope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
    }
}