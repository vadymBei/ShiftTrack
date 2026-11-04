using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

namespace ShiftTrack.Infrastructure.Modules.Communication.Messenger.Services;

/// <summary>
/// Removes uploads that were never sent, every hour. Runs in the API, where the files are
/// (ShiftTrack.Jobs has no access to its disk).
/// </summary>
public class AttachmentCleanupService(
    IServiceScopeFactory scopeFactory,
    ILogger<AttachmentCleanupService> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);

        do
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                await scope.ServiceProvider.GetRequiredService<IAttachmentCleanup>().RemoveUnsent(stoppingToken);
            }
            catch (Exception exception) when (!stoppingToken.IsCancellationRequested)
            {
                logger.LogError("{LogName} Failed to remove unsent attachments: {Error}",
                    "RemoveUnsentAttachments", exception.Message);
            }
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}
