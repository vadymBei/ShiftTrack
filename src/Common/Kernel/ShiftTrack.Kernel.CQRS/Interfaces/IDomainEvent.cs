namespace ShiftTrack.Kernel.CQRS.Interfaces;

/// <summary>
/// Доменна подія (DDD). Розширює <see cref="INotification"/>,
/// тому будь-який <see cref="IDomainEventHandler{T}"/> автоматично
/// є <see cref="INotificationHandler{T}"/> і публікується через <see cref="IMediator.Publish"/>.
/// </summary>
public interface IDomainEvent : INotification;