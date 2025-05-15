using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using IMediator = ShiftTrack.Kernel.CQRS.Interfaces.IMediator;

namespace ShiftTrack.Core.Application.Integration.Tests.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _serviceScope;
    protected readonly ISender Sender;
    protected readonly IMediator Mediator;
    protected readonly IApplicationDbContext DbContext;

    protected BaseIntegrationTest(
        IntegrationTestWebAppFactory factory)
    {
        _serviceScope = factory.Services.CreateScope();
            
        Sender = _serviceScope.ServiceProvider.GetRequiredService<ISender>();
        Mediator = _serviceScope.ServiceProvider.GetRequiredService<IMediator>();
        DbContext = _serviceScope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
    }
}