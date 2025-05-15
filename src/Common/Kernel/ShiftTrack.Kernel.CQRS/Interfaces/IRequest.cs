namespace ShiftTrack.Kernel.CQRS.Interfaces;

public interface IRequest
{
}

public interface IRequest<out TResponse> : IRequest
{
}