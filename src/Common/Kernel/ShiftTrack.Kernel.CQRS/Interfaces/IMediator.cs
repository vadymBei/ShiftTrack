namespace ShiftTrack.Kernel.CQRS.Interfaces;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task Send(IRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Публікує одне повідомлення всім зареєстрованим <see cref="INotificationHandler{T}"/>.
    /// Обробники виконуються паралельно.
    /// </summary>
    Task Publish(INotification notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Публікує декілька повідомлень. Кожне повідомлення обробляється паралельно.
    /// </summary>
    Task Publish(IEnumerable<INotification> notifications, CancellationToken cancellationToken = default);
}