using Quartz;
using ShiftTrack.Application.Common.Outbox.Interfaces;
using ShiftTrack.Scheduling.Jobs;

namespace ShiftTrack.Jobs.Jobs.Outbox;

/// <summary>
/// Publishes domain events saved to the outbox (notifications, etc.) to their handlers.
/// Runs every few seconds; safe with several instances of ShiftTrack.Jobs.
/// </summary>
public class OutboxProcessorJob(
    IOutboxProcessor outboxProcessor,
    ILogger<OutboxProcessorJob> logger) : ScheduledJob(logger)
{
    protected override LogLevel ExecutionLogLevel => LogLevel.Debug;

    protected override Task ExecuteJob(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        return outboxProcessor.ProcessPending(cancellationToken);
    }
}
