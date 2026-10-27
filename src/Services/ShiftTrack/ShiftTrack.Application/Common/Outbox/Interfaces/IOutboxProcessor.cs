namespace ShiftTrack.Application.Common.Outbox.Interfaces;

public interface IOutboxProcessor
{
    /// <summary>Publishes pending domain events to their handlers. Returns the number of processed messages.</summary>
    Task<int> ProcessPending(CancellationToken cancellationToken);

    /// <summary>Removes processed messages older than the retention period.</summary>
    Task<int> CleanUp(CancellationToken cancellationToken);
}
