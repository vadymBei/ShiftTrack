namespace ShiftTrack.Kernel.CQRS.Interfaces;

/// <summary>
/// Обробник повідомлення типу <typeparamref name="TNotification"/>.
/// Один notification може мати декілька незалежних обробників —
/// всі виконуються паралельно через <see cref="IMediator"/>.
/// </summary>
public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    Task Handle(TNotification notification, CancellationToken cancellationToken = default);
}