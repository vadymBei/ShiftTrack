using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using IMediator = ShiftTrack.Kernel.CQRS.Interfaces.IMediator;

namespace ShiftTrack.Core.Application.Integration.Tests.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _serviceScope;
    protected readonly IMediator Mediator;
    protected readonly IApplicationDbContext DbContext;

    protected BaseIntegrationTest(
        IntegrationTestWebAppFactory factory)
    {
        _serviceScope = factory.Services.CreateScope();
            
        Mediator = _serviceScope.ServiceProvider.GetRequiredService<IMediator>();
        DbContext = _serviceScope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
    }
}