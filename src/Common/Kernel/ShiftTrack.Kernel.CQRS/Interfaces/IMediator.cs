namespace ShiftTrack.Kernel.CQRS.Interfaces;

public interface IMediator
{
    Task<TResponse> Invoke<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task Invoke(IRequest request, CancellationToken cancellationToken = default);
}