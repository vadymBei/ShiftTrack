namespace ShiftTrack.Kernel.CQRS.Interfaces;

public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default);    
}