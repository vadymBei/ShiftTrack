namespace ShiftTrack.Kernel.CQRS.Interfaces;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvent, CancellationToken cancellationToken = default);
}