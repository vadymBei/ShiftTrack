namespace ShiftTrack.Kernel.CQRS.Interfaces;

/// <summary>
/// Обробник доменної події. Є спеціалізацією <see cref="INotificationHandler{TDomainEvent}"/>.
/// Зареєстровані обробники автоматично резолвляться через <see cref="IMediator"/>.
/// </summary>
public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;