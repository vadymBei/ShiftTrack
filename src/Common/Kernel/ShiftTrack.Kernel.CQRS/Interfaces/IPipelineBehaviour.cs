namespace ShiftTrack.Kernel.CQRS.Interfaces;

public interface IPipelineBehaviour<in TRequest, TResponse>
{
    Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken);
}

/// <summary>
/// Delegate that passes the CancellationToken through the pipeline chain,
/// allowing each behavior to intercept or replace the token.
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