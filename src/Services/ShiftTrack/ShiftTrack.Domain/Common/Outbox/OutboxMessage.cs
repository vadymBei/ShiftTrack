namespace ShiftTrack.Domain.Common.Outbox;

/// <summary>
/// A domain event saved in the same transaction as the business change (transactional outbox).
/// The outbox processor in ShiftTrack.Jobs publishes it to the in-process handlers afterwards.
/// </summary>
public class OutboxMessage
{
    public long Id { get; set; }

    /// <summary>Event type as "Namespace.TypeName, AssemblyName".</summary>
    public string Type { get; set; }

    /// <summary>Event serialized to JSON.</summary>
    public string Content { get; set; }

    /// <summary>Employee whose action raised the event, handlers run on their behalf.</summary>
    public long? ActorEmployeeId { get; set; }

    public DateTime OccurredAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public int Attempts { get; set; }

    /// <summary>Not earlier than this time the failed message is retried.</summary>
    public DateTime? NextAttemptAt { get; set; }

    /// <summary>The message is being processed by an instance until this time.</summary>
    public DateTime? LockExpiresAt { get; set; }

    public string Error { get; set; }
}
