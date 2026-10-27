using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Common.Behaviours;

/// <summary>
/// Every command runs in one database transaction, so the business change and the domain events
/// saved to the outbox are committed together or not at all.
/// </summary>
public class TransactionBehaviour<TRequest, TResponse>(
    IUnitOfWork unitOfWork) : IPipelineBehaviour<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!CommandConvention.IsCommand(typeof(TRequest)))
            return next(cancellationToken);

        return unitOfWork.ExecuteInTransaction(() => next(cancellationToken), cancellationToken);
    }
}

/// <inheritdoc cref="TransactionBehaviour{TRequest,TResponse}"/>
public class TransactionBehaviour<TRequest>(
    IUnitOfWork unitOfWork) : IPipelineBehaviour<TRequest>
{
    public Task Handle(
        TRequest request,
        RequestHandlerDelegate next,
        CancellationToken cancellationToken)
    {
        if (!CommandConvention.IsCommand(typeof(TRequest)))
            return next(cancellationToken);

        return unitOfWork.ExecuteInTransaction(async () =>
        {
            await next(cancellationToken);
            return true;
        }, cancellationToken);
    }
}

internal static class CommandConvention
{
    /// <summary>Commands live in "*.UseCases.Commands.*", queries don't change data and don't need a transaction.</summary>
    public static bool IsCommand(Type requestType) =>
        requestType.Namespace?.Contains(".UseCases.Commands", StringComparison.Ordinal) == true;
}
