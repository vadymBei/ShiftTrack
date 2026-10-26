namespace ShiftTrack.Kernel.CQRS.Interfaces;

/// <summary>
/// Handler for notifications of type <typeparamref name="TNotification"/>.
/// A single notification can have multiple independent handlers,
/// they are executed sequentially via <see cref="IMediator"/>.
/// </summary>
public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    Task Handle(TNotification notification, CancellationToken cancellationToken = default);
}