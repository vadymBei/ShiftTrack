namespace ShiftTrack.Kernel.CQRS.Interfaces;

/// <summary>
/// Domain event handler. A specialization of <see cref="INotificationHandler{TDomainEvent}"/>.
/// Registered handlers are automatically resolved through <see cref="IMediator"/>.
/// </summary>
public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;