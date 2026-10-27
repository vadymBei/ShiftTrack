namespace ShiftTrack.Application.Common.Outbox.Constants;

public static class OutboxConstants
{
    /// <summary>Messages taken by one processor run.</summary>
    public const int BatchSize = 50;

    /// <summary>Batches processed in a row before the run ends.</summary>
    public const int MaxBatchesPerRun = 10;

    /// <summary>After this many failures the message is left for investigation.</summary>
    public const int MaxAttempts = 10;

    /// <summary>A claimed message is hidden from other instances for this time.</summary>
    public static readonly TimeSpan LockDuration = TimeSpan.FromMinutes(5);

    public static readonly TimeSpan FirstRetryDelay = TimeSpan.FromSeconds(10);
    public static readonly TimeSpan MaxRetryDelay = TimeSpan.FromHours(1);

    /// <summary>Processed messages are kept for this time and then removed.</summary>
    public static readonly TimeSpan ProcessedRetention = TimeSpan.FromDays(7);

    public const int MaxErrorLength = 4000;
}
