namespace ShiftTrack.Kernel.CQRS.Interfaces;

public interface IPipelineBehaviour<in TRequest, TResponse>
{
    Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken);
}

/// <summary>
/// Делегат, що передає CancellationToken через ланцюжок pipeline,
/// дозволяючи кожному behavior перехопити або замінити токен.
/// </summary>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>(CancellationToken cancellationToken = default);

public interface IPipelineBehaviour<in TRequest>
{
    Task Handle(
        TRequest request,
        RequestHandlerDelegate next,
        CancellationToken cancellationToken);
}

/// <inheritdoc cref="RequestHandlerDelegate{TResponse}"/>
public delegate Task RequestHandlerDelegate(CancellationToken cancellationToken = default);