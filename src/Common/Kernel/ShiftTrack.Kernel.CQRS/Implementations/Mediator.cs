using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Kernel.CQRS.Implementations;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResponse> Invoke<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        var (handler, handleMethod) = GetHandler(requestType, responseType);
        var (pipelines, pipelineMethod) = GetPipelines(requestType, responseType);

        if (!pipelines.Any())
        {
            return await InvokeHandler<TResponse>(
                handler,
                handleMethod,
                request,
                cancellationToken);
        }

        return await BuildAndExecutePipeline<TResponse>(
            request,
            handler,
            handleMethod,
            pipelines,
            pipelineMethod,
            cancellationToken);
    }

    public async Task Invoke(IRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();

        var (handler, handleMethod) = GetHandler(requestType, typeof(void));
        var (pipelines, pipelineMethod) = GetPipelines(requestType, typeof(void));

        if (!pipelines.Any())
        {
            await InvokeHandler(
                handler,
                handleMethod,
                request,
                cancellationToken);

            return;
        }

        await BuildAndExecutePipeline(
            request,
            handler,
            handleMethod,
            pipelines,
            pipelineMethod,
            cancellationToken);
    }

    private (object Handler, MethodInfo HandleMethod) GetHandler(Type requestType, Type responseType)
    {
        var handlerType = GetHandlerType(requestType, responseType);
        var handler = serviceProvider.GetRequiredService(handlerType);

        var handleMethod = handlerType.GetMethod(nameof(IRequestHandler<IRequest>.Handle))
                           ?? throw CreateHandleMethodNotFoundException(handlerType.Name);

        return (handler, handleMethod);
    }

    private (object[] Pipelines, MethodInfo PipelineMethod) GetPipelines(Type requestType, Type responseType)
    {
        Type pipelineType;

        if (responseType == typeof(void))
        {
            pipelineType = typeof(IPipelineBehaviour<>).MakeGenericType(requestType);
        }
        else
        {
            pipelineType = typeof(IPipelineBehaviour<,>).MakeGenericType(requestType, responseType);
        }

        var pipelines = serviceProvider.GetServices(pipelineType).ToArray();
        var pipelineMethod = pipelineType.GetMethod(nameof(IPipelineBehaviour<IRequest, object>.Handle))
                             ?? throw CreateHandleMethodNotFoundException(pipelineType.Name);

        return (pipelines, pipelineMethod);
    }

    private static async Task<TResponse> BuildAndExecutePipeline<TResponse>(
        object request,
        object handler,
        MethodInfo handleMethod,
        object[] pipelines,
        MethodInfo pipelineMethod,
        CancellationToken cancellationToken)
    {
        RequestHandlerDelegate<TResponse> baseHandler = () =>
            (Task<TResponse>)handleMethod.Invoke(handler, new[] { request, cancellationToken });

        var pipeline = pipelines
            .Aggregate(
                baseHandler,
                (next, pipe) =>
                {
                    var currentNext = next;

                    return new RequestHandlerDelegate<TResponse>(() =>
                        (Task<TResponse>)pipelineMethod.Invoke(
                            pipe,
                            new object[] { request, currentNext, cancellationToken }));
                });

        return await pipeline();
    }

    private static async Task BuildAndExecutePipeline(
        object request,
        object handler,
        MethodInfo handleMethod,
        object[] pipelines,
        MethodInfo pipelineMethod,
        CancellationToken cancellationToken)
    {
        RequestHandlerDelegate baseHandler = () =>
            (Task)handleMethod.Invoke(handler, new[] { request, cancellationToken });

        var pipeline = pipelines
            .Aggregate(
                baseHandler,
                (next, pipe) =>
                {
                    var currentNext = next;
                    return new RequestHandlerDelegate(() =>
                        (Task)pipelineMethod.Invoke(
                            pipe,
                            new object[] { request, currentNext, cancellationToken }));
                });

        await pipeline();
    }

    private static async Task<TResponse> InvokeHandler<TResponse>(
        object handler,
        MethodInfo handleMethod,
        object request,
        CancellationToken cancellationToken)
    {
        return await (Task<TResponse>)handleMethod.Invoke(handler, new[] { request, cancellationToken });
    }

    private static async Task InvokeHandler(
        object handler,
        MethodInfo handleMethod,
        object request,
        CancellationToken cancellationToken)
    {
        await (Task)handleMethod.Invoke(handler, new[] { request, cancellationToken });
    }

    private static Type GetHandlerType(Type requestType, Type responseType) =>
        responseType == typeof(void)
            ? typeof(IRequestHandler<>).MakeGenericType(requestType)
            : typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

    private static Type GetPipelineType(Type requestType, Type responseType) =>
        typeof(IPipelineBehaviour<,>).MakeGenericType(requestType, responseType);

    private static InvalidOperationException CreateHandleMethodNotFoundException(string typeName) =>
        new($"Handle method not found on {typeName}");
}