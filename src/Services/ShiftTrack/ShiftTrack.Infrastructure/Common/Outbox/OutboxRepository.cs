using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Outbox.Interfaces;
using ShiftTrack.Domain.Common.Outbox;
using ShiftTrack.Infrastructure.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Common.Outbox;

public class OutboxRepository(
    IApplicationDbContext applicationDbContext) : IOutboxRepository
{
    public async Task Add(OutboxMessage message, CancellationToken cancellationToken)
    {
        await applicationDbContext.OutboxMessages.AddAsync(message, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OutboxMessage>> Claim(int batchSize, TimeSpan lockDuration, int maxAttempts,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        await using var transaction = await applicationDbContext.Database.BeginTransactionAsync(cancellationToken);

        // SKIP LOCKED: rows taken by another instance are skipped instead of waited for.
        var messages = await applicationDbContext.OutboxMessages
            .FromSql($"""
                      SELECT * FROM "OutboxMessages"
                      WHERE "ProcessedAt" IS NULL
                        AND "Attempts" < {maxAttempts}
                        AND ("NextAttemptAt" IS NULL OR "NextAttemptAt" <= {now})
                        AND ("LockExpiresAt" IS NULL OR "LockExpiresAt" <= {now})
                      ORDER BY "Id"
                      LIMIT {batchSize}
                      FOR UPDATE SKIP LOCKED
                      """)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
            message.LockExpiresAt = now + lockDuration;

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return messages;
    }

    public Task MarkProcessed(long id, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        return applicationDbContext.OutboxMessages
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.ProcessedAt, now)
                .SetProperty(x => x.LockExpiresAt, (DateTime?)null)
                .SetProperty(x => x.Error, (string)null), cancellationToken);
    }

    public Task MarkFailed(long id, int attempts, DateTime nextAttemptAt, string error,
        CancellationToken cancellationToken)
    {
        return applicationDbContext.OutboxMessages
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.Attempts, attempts)
                .SetProperty(x => x.NextAttemptAt, nextAttemptAt)
                .SetProperty(x => x.LockExpiresAt, (DateTime?)null)
                .SetProperty(x => x.Error, error), cancellationToken);
    }

    public Task<int> DeleteProcessed(DateTime processedBefore, CancellationToken cancellationToken)
    {
        return applicationDbContext.OutboxMessages
            .Where(x => x.ProcessedAt != null && x.ProcessedAt < processedBefore)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
