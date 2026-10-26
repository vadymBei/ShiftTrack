using Quartz;
using ShiftTrack.Application.Common.Outbox.Interfaces;
using ShiftTrack.Scheduling.Jobs;

namespace ShiftTrack.Jobs.Jobs.Outbox;

/// <summary>Removes processed outbox messages after the retention period.</summary>
public class OutboxCleanupJob(
    IOutboxProcessor outboxProcessor,
    ILogger<OutboxCleanupJob> logger) : ScheduledJob(logger)
{
    protected override async Task ExecuteJob(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        var deleted = await outboxProcessor.CleanUp(cancellationToken);

        logger.LogInformation("Removed {Count} processed outbox messages", deleted);
    }
}
