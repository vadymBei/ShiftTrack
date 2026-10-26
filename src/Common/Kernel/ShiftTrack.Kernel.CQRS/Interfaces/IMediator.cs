namespace ShiftTrack.Kernel.CQRS.Interfaces;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task Send(IRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a single notification to all registered <see cref="INotificationHandler{T}"/>.
    /// Handlers are executed sequentially in the caller's scope, so they can safely share scoped services
    /// such as DbContext. An exception in a handler stops the remaining ones.
    /// </summary>
    Task Publish(INotification notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes multiple notifications one after another, in the given order.
    /// </summary>
    Task Publish(IEnumerable<INotification> notifications, CancellationToken cancellationToken = default);
}