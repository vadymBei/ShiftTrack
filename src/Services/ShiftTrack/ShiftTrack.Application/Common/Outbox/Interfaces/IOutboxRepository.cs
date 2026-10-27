using ShiftTrack.Domain.Common.Outbox;

namespace ShiftTrack.Application.Common.Outbox.Interfaces;

public interface IOutboxRepository
{
    /// <summary>Saves the message, in the current transaction if there is one.</summary>
    Task Add(OutboxMessage message, CancellationToken cancellationToken);

    /// <summary>
    /// Takes due messages and locks them for <paramref name="lockDuration"/>,
    /// so several processor instances don't take the same messages.
    /// </summary>
    Task<IReadOnlyList<OutboxMessage>> Claim(int batchSize, TimeSpan lockDuration, int maxAttempts,
        CancellationToken cancellationToken);

    Task MarkProcessed(long id, CancellationToken cancellationToken);

    Task MarkFailed(long id, int attempts, DateTime nextAttemptAt, string error, CancellationToken cancellationToken);

    Task<int> DeleteProcessed(DateTime processedBefore, CancellationToken cancellationToken);
}
