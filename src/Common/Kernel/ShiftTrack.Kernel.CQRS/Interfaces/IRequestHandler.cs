﻿namespace ShiftTrack.Kernel.CQRS.Interfaces;

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
    Task Handle(TRequest request, CancellationToken cancellationToken = default);
}