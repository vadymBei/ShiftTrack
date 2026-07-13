namespace ShiftTrack.Kernel.CQRS.Interfaces;

/// <summary>
/// Domain event (DDD). Extends <see cref="INotification"/>,
/// so any <see cref="IDomainEventHandler{T}"/> is automatically
/// an <see cref="INotificationHandler{T}"/> and is published via <see cref="IMediator.Publish"/>.
/// </summary>
public interface IDomainEvent : INotification;