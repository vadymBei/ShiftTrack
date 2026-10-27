using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Common.Outbox.Interfaces;

/// <summary>
/// Saves domain events to the outbox in the current transaction instead of publishing them in-process.
/// Handlers run later in ShiftTrack.Jobs, so the request doesn't wait for them and an event isn't lost
/// if the process stops right after the business change.
/// </summary>
public interface IDomainEventOutbox
{
    Task Enqueue(IDomainEvent domainEvent, CancellationToken cancellationToken);
}
