using System.Collections.Concurrent;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Kernel.CQRS.Implementations;

public class Mediator(
    IServiceProvider serviceProvider,
    ICancellationTokenProvider cancellationTokenProvider) : IMediator
{
    // ── Compiled delegate caches ──────────────────────────────────────────────
    // Expression.Compile() викликається лише ОДИН РАЗ на тип запиту/повідомлення.
    // Після цього — виклик через делегат, що є таким самим швидким як звичайний virtual dispatch.
    // Порівняно з MethodInfo.Invoke: немає boxing аргументів у object[], немає reflection overhead.

    // Func<object handler, object request, CancellationToken, Task>
    private static readonly ConcurrentDictionary<Type, (Type ServiceType, Func<object, object, CancellationToken, Task>
            Invoke)>
        HandlerCache = new();

    private static readonly ConcurrentDictionary<Type, (Type ServiceType, Func<object, object, CancellationToken, Task>
            Invoke)>
        VoidHandlerCache = new();

    // Func<object pipe, object request, object next, CancellationToken, Task>
    private static readonly ConcurrentDictionary<Type, (Type ServiceType,
            Func<object, object, object, CancellationToken, Task> Invoke)>
        PipelineCache = new();

    private static readonly ConcurrentDictionary<Type, (Type ServiceType,
            Func<object, object, object, CancellationToken, Task> Invoke)>
        VoidPipelineCache = new();

    // Func<object handler, object notification, CancellationToken, Task>
    private static readonly ConcurrentDictionary<Type, (Type ServiceType, Func<object, object, CancellationToken, Task>
            Invoke)>
        NotificationCache = new();

    // ── Expression compilers ──────────────────────────────────────────────────

    /// <summary>
    /// Будує і компілює expression:
    /// <c>(handler, request, ct) => ((IRequestHandler&lt;TReq, TRes&gt;)handler).Handle((TReq)request, ct)</c>
    /// </summary>
    private static Func<object, object, CancellationToken, Task> CompileHandlerInvoker(
        Type requestType, Type responseType)
    {
        var iface = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
        var method = iface.GetMethod("Handle")!;

        var handlerParam = Expression.Parameter(typeof(object), "handler");
        var requestParam = Expression.Parameter(typeof(object), "request");
        var ctParam = Expression.Parameter(typeof(CancellationToken), "ct");

        var body = Expression.Call(
            Expression.Convert(handlerParam, iface),
            method,
            Expression.Convert(requestParam, requestType),
            ctParam);

        return Expression.Lambda<Func<object, object, CancellationToken, Task>>(
            body, handlerParam, requestParam, ctParam).Compile();
    }

    /// <summary>
    /// Будує і компілює expression:
    /// <c>(handler, request, ct) => ((IRequestHandler&lt;TReq&gt;)handler).Handle((TReq)request, ct)</c>
    /// </summary>
    private static Func<object, object, CancellationToken, Task> CompileVoidHandlerInvoker(Type requestType)
    {
        var iface = typeof(IRequestHandler<>).MakeGenericType(requestType);
        var method = iface.GetMethod("Handle")!;

        var handlerParam = Expression.Parameter(typeof(object), "handler");
        var requestParam = Expression.Parameter(typeof(object), "request");
        var ctParam = Expression.Parameter(typeof(CancellationToken), "ct");

        var body = Expression.Call(
            Expression.Convert(handlerParam, iface),
            method,
            Expression.Convert(requestParam, requestType),
            ctParam);

        return Expression.Lambda<Func<object, object, CancellationToken, Task>>(
            body, handlerParam, requestParam, ctParam).Compile();
    }

    /// <summary>
    /// Будує і компілює expression:
    /// <c>(pipe, request, next, ct) => ((IPipelineBehaviour&lt;TReq, TRes&gt;)pipe).Handle((TReq)request, (RequestHandlerDelegate&lt;TRes&gt;)next, ct)</c>
    /// </summary>
    private static Func<object, object, object, CancellationToken, Task> CompilePipelineInvoker(
        Type requestType, Type responseType)
    {
        var iface = typeof(IPipelineBehaviour<,>).MakeGenericType(requestType, responseType);
        var delegateType = typeof(RequestHandlerDelegate<>).MakeGenericType(responseType);
        var method = iface.GetMethod("Handle")!;

        var pipeParam = Expression.Parameter(typeof(object), "pipe");
        var requestParam = Expression.Parameter(typeof(object), "request");
        var nextParam = Expression.Parameter(typeof(object), "next");
        var ctParam = Expression.Parameter(typeof(CancellationToken), "ct");

        var body = Expression.Call(
            Expression.Convert(pipeParam, iface),
            method,
            Expression.Convert(requestParam, requestType),
            Expression.Convert(nextParam, delegateType),
            ctParam);

        return Expression.Lambda<Func<object, object, object, CancellationToken, Task>>(
            body, pipeParam, requestParam, nextParam, ctParam).Compile();
    }

    /// <summary>
    /// Будує і компілює expression:
    /// <c>(pipe, request, next, ct) => ((IPipelineBehaviour&lt;TReq&gt;)pipe).Handle((TReq)request, (RequestHandlerDelegate)next, ct)</c>
    /// </summary>
    private static Func<object, object, object, CancellationToken, Task> CompileVoidPipelineInvoker(Type requestType)
    {
        var iface = typeof(IPipelineBehaviour<>).MakeGenericType(requestType);
        var method = iface.GetMethod("Handle")!;

        var pipeParam = Expression.Parameter(typeof(object), "pipe");
        var requestParam = Expression.Parameter(typeof(object), "request");
        var nextParam = Expression.Parameter(typeof(object), "next");
        var ctParam = Expression.Parameter(typeof(CancellationToken), "ct");

        var body = Expression.Call(
            Expression.Convert(pipeParam, iface),
            method,
            Expression.Convert(requestParam, requestType),
            Expression.Convert(nextParam, typeof(RequestHandlerDelegate)),
            ctParam);

        return Expression.Lambda<Func<object, object, object, CancellationToken, Task>>(
            body, pipeParam, requestParam, nextParam, ctParam).Compile();
    }

    /// <summary>
    /// Будує і компілює expression:
    /// <c>(handler, notification, ct) => ((INotificationHandler&lt;TNotif&gt;)handler).Handle((TNotif)notification, ct)</c>
    /// </summary>
    private static Func<object, object, CancellationToken, Task> CompileNotificationInvoker(Type notificationType)
    {
        var iface = typeof(INotificationHandler<>).MakeGenericType(notificationType);
        var method = iface.GetMethod("Handle")!;

        var handlerParam = Expression.Parameter(typeof(object), "handler");
        var notificationParam = Expression.Parameter(typeof(object), "notification");
        var ctParam = Expression.Parameter(typeof(CancellationToken), "ct");

        var body = Expression.Call(
            Expression.Convert(handlerParam, iface),
            method,
            Expression.Convert(notificationParam, notificationType),
            ctParam);

        return Expression.Lambda<Func<object, object, CancellationToken, Task>>(
            body, handlerParam, notificationParam, ctParam).Compile();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Якщо токен не переданий явно (default) — автоматично бере з HttpContext.RequestAborted.
    /// </summary>
    private CancellationToken ResolveToken(CancellationToken cancellationToken) =>
        cancellationToken == default ? cancellationTokenProvider.Token : cancellationToken;

    // ── Public API ────────────────────────────────────────────────────────────

    public async Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken = ResolveToken(cancellationToken);

        var requestType = request.GetType();

        var (handlerType, handlerInvoke) = HandlerCache.GetOrAdd(requestType, t =>
        {
            var ht = typeof(IRequestHandler<,>).MakeGenericType(t, typeof(TResponse));
            return (ht, CompileHandlerInvoker(t, typeof(TResponse)));
        });

        var handler = serviceProvider.GetRequiredService(handlerType);

        var (pipelineType, pipelineInvoke) = PipelineCache.GetOrAdd(requestType, t =>
        {
            var pt = typeof(IPipelineBehaviour<,>).MakeGenericType(t, typeof(TResponse));
            return (pt, CompilePipelineInvoker(t, typeof(TResponse)));
        });

        var pipelines = serviceProvider.GetServices(pipelineType).ToArray();

        if (pipelines.Length == 0)
            return await (Task<TResponse>)handlerInvoke(handler, request, cancellationToken);

        // Будуємо ланцюжок: кожен behavior отримує "наступний" крок як делегат
        RequestHandlerDelegate<TResponse> baseHandler =
            ct => (Task<TResponse>)handlerInvoke(handler, request, ct);

        var pipeline = pipelines.Aggregate(
            baseHandler,
            (next, pipe) =>
            {
                var captured = next;
                return new RequestHandlerDelegate<TResponse>(ct =>
                    (Task<TResponse>)pipelineInvoke(pipe!, request, captured, ct));
            });

        return await pipeline(cancellationToken);
    }

    public async Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken = ResolveToken(cancellationToken);

        var requestType = request.GetType();

        var (handlerType, handlerInvoke) = VoidHandlerCache.GetOrAdd(requestType, t =>
        {
            var ht = typeof(IRequestHandler<>).MakeGenericType(t);
            return (ht, CompileVoidHandlerInvoker(t));
        });

        var handler = serviceProvider.GetRequiredService(handlerType);

        var (pipelineType, pipelineInvoke) = VoidPipelineCache.GetOrAdd(requestType, t =>
        {
            var pt = typeof(IPipelineBehaviour<>).MakeGenericType(t);
            return (pt, CompileVoidPipelineInvoker(t));
        });

        var pipelines = serviceProvider.GetServices(pipelineType).ToArray();

        if (pipelines.Length == 0)
        {
            await handlerInvoke(handler, request, cancellationToken);
            return;
        }

        RequestHandlerDelegate baseHandler =
            ct => handlerInvoke(handler, request, ct);

        var pipeline = pipelines.Aggregate(
            baseHandler,
            (next, pipe) =>
            {
                var captured = next;
                return new RequestHandlerDelegate(ct =>
                    pipelineInvoke(pipe!, request, captured, ct));
            });

        await pipeline(cancellationToken);
    }

    public Task Publish(INotification notification, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(notification);
        cancellationToken = ResolveToken(cancellationToken);

        var notificationType = notification.GetType();

        var (handlerType, handlerInvoke) = NotificationCache.GetOrAdd(notificationType, t =>
        {
            var ht = typeof(INotificationHandler<>).MakeGenericType(t);
            return (ht, CompileNotificationInvoker(t));
        });

        var handlers = serviceProvider.GetServices(handlerType).ToArray();

        if (handlers.Length == 0)
            return Task.CompletedTask;

        // Всі обробники одного повідомлення незалежні — виконуємо паралельно
        var tasks = handlers
            .Where(h => h is not null)
            .Select(h => handlerInvoke(h!, notification, cancellationToken));

        return Task.WhenAll(tasks);
    }

    public Task Publish(IEnumerable<INotification> notifications, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(notifications);

        return Task.WhenAll(notifications.Select(n => Publish(n, cancellationToken)));
    }
}