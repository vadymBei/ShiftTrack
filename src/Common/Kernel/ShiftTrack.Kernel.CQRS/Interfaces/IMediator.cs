namespace ShiftTrack.Kernel.CQRS.Interfaces;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task Send(IRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a single notification to all registered <see cref="INotificationHandler{T}"/>.
    /// Handlers are executed in parallel.
    /// </summary>
    Task Publish(INotification notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes multiple notifications. Each notification is processed in parallel.
    /// </summary>
    Task Publish(IEnumerable<INotification> notifications, CancellationToken cancellationToken = default);
}