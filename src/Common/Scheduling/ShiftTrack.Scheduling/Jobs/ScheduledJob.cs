using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ShiftTrack.Scheduling.Jobs;

/// <summary>
/// Base class for scheduled jobs: logs start, finish and duration, and turns failures
/// into <see cref="JobExecutionException"/> so one failed run doesn't stop the scheduler.
/// Every run gets its own DI scope, so scoped services (DbContext, repositories) can be injected.
/// Concurrent runs of the same job are not allowed.
/// </summary>
[DisallowConcurrentExecution]
public abstract class ScheduledJob(ILogger logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var jobName = context.JobDetail.Key.Name;
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation("Job {JobName} started", jobName);

        try
        {
            await ExecuteJob(context, context.CancellationToken);

            logger.LogInformation("Job {JobName} finished in {ElapsedMilliseconds} ms",
                jobName, stopwatch.ElapsedMilliseconds);
        }
        catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested)
        {
            logger.LogWarning("Job {JobName} was cancelled after {ElapsedMilliseconds} ms",
                jobName, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Job {JobName} failed after {ElapsedMilliseconds} ms",
                jobName, stopwatch.ElapsedMilliseconds);

            throw new JobExecutionException(exception, refireImmediately: false);
        }
    }

    protected abstract Task ExecuteJob(IJobExecutionContext context, CancellationToken cancellationToken);
}
