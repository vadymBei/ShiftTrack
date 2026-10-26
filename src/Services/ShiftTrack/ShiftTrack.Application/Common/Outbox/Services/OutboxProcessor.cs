using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.Outbox.Constants;
using ShiftTrack.Application.Common.Outbox.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Domain.Common.Outbox;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Common.Outbox.Services;

/// <summary>
/// Publishes outbox messages to the in-process handlers. Delivery is at-least-once: a message is retried with
/// a growing delay if a handler fails, and can be handled again if the process stops before it's marked.
/// </summary>
public class OutboxProcessor(
    IOutboxRepository outboxRepository,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<OutboxProcessor> logger) : IOutboxProcessor
{
    public async Task<int> ProcessPending(CancellationToken cancellationToken)
    {
        var processed = 0;

        for (var batch = 0; batch < OutboxConstants.MaxBatchesPerRun; batch++)
        {
            var messages = await outboxRepository.Claim(
                OutboxConstants.BatchSize, OutboxConstants.LockDuration, OutboxConstants.MaxAttempts,
                cancellationToken);

            foreach (var message in messages)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Process(message, cancellationToken);
            }

            processed += messages.Count;

            if (messages.Count < OutboxConstants.BatchSize)
                break;
        }

        if (processed > 0)
            logger.LogInformation("Processed {Count} outbox messages", processed);

        return processed;
    }

    public Task<int> CleanUp(CancellationToken cancellationToken)
    {
        return outboxRepository.DeleteProcessed(DateTime.UtcNow - OutboxConstants.ProcessedRetention,
            cancellationToken);
    }

    private async Task Process(OutboxMessage message, CancellationToken cancellationToken)
    {
        try
        {
            var domainEvent = OutboxSerializer.Deserialize(message.Type, message.Content);

            // A scope per message: handlers get their own DbContext and the actor of this event only.
            await using var scope = serviceScopeFactory.CreateAsyncScope();

            await SetActor(scope.ServiceProvider, message.ActorEmployeeId, cancellationToken);

            await scope.ServiceProvider
                .GetRequiredService<IMediator>()
                .Publish(domainEvent, cancellationToken);

            await outboxRepository.MarkProcessed(message.Id, cancellationToken);
        }
        catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
        {
            var attempts = message.Attempts + 1;
            var delay = GetRetryDelay(attempts);

            logger.LogError(exception,
                "Outbox message {MessageId} ({Type}) failed, attempt {Attempt} of {MaxAttempts}",
                message.Id, message.Type, attempts, OutboxConstants.MaxAttempts);

            var error = exception.ToString();

            await outboxRepository.MarkFailed(
                message.Id,
                attempts,
                DateTime.UtcNow + delay,
                error.Length > OutboxConstants.MaxErrorLength ? error[..OutboxConstants.MaxErrorLength] : error,
                cancellationToken);
        }
    }

    /// <summary>Handlers run on behalf of the employee who raised the event (sender and audit author).</summary>
    private static async Task SetActor(IServiceProvider serviceProvider, long? actorEmployeeId,
        CancellationToken cancellationToken)
    {
        if (actorEmployeeId is null)
            return;

        var employees = await serviceProvider
            .GetRequiredService<IEmployeeRepository>()
            .GetListByIds([actorEmployeeId.Value], cancellationToken);

        serviceProvider.GetRequiredService<ICurrentUserService>().Employee = employees.FirstOrDefault();
    }

    private static TimeSpan GetRetryDelay(int attempts)
    {
        var seconds = OutboxConstants.FirstRetryDelay.TotalSeconds * Math.Pow(2, attempts - 1);

        return TimeSpan.FromSeconds(Math.Min(seconds, OutboxConstants.MaxRetryDelay.TotalSeconds));
    }
}
