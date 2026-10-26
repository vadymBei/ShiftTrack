using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.Outbox.Interfaces;
using ShiftTrack.Domain.Common.Outbox;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Common.Outbox.Services;

public class DomainEventOutbox(
    ICurrentUserService currentUserService,
    IOutboxRepository outboxRepository) : IDomainEventOutbox
{
    public Task Enqueue(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        return outboxRepository.Add(new OutboxMessage
        {
            Type = OutboxSerializer.GetTypeName(domainEvent.GetType()),
            Content = OutboxSerializer.Serialize(domainEvent),
            ActorEmployeeId = currentUserService.Employee?.Id,
            OccurredAt = DateTime.UtcNow
        }, cancellationToken);
    }
}
