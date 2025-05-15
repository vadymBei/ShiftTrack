using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Kernel.CQRS;

public class Mediator(
    IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResponse> Invoke<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        
        var handlerType = typeof(IRequestHandler<,> ).MakeGenericType(requestType, typeof(TResponse));
        
        var handler = serviceProvider.GetRequiredService(handlerType);
        
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler for request of type {requestType.Name} not found");
        }

        var handleMethod = handlerType.GetMethod("Handle");
        
        if (handleMethod == null)
        {
            throw new InvalidOperationException($"Handle method not found on {handlerType.Name}");
        }

        return await (Task<TResponse>)handleMethod
            .Invoke(handler, new object[] { request, cancellationToken });
    }

    public async Task Invoke(IRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        
        var handlerType = typeof(IRequestHandler<> )
            .MakeGenericType(requestType);
        
        var handler = serviceProvider.GetRequiredService(handlerType);
        
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler for request of type {requestType.Name} not found");
        }

        var handleMethod = handlerType.GetMethod("Handle");
        
        if (handleMethod == null)
        {
            throw new InvalidOperationException($"Handle method not found on {handlerType.Name}");
        }

        await (Task)handleMethod
            .Invoke(handler, new object[] { request, cancellationToken });
    }
}