namespace ShiftTrack.Kernel.CQRS.Interfaces;

public interface IPipelineBehaviour<in TRequest, TResponse>
{
    Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken);
}

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

public interface IPipelineBehaviour<in TRequest>
{
    Task Handle(
        TRequest request,
        RequestHandlerDelegate next,
        CancellationToken cancellationToken);
}

public delegate Task RequestHandlerDelegate();

