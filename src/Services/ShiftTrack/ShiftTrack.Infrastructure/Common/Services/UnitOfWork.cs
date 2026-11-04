using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Infrastructure.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Common.Services;

public class UnitOfWork(
    IApplicationDbContext applicationDbContext,
    ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private readonly List<Func<Task>> _afterCommit = [];

    public async Task<T> ExecuteInTransaction<T>(Func<Task<T>> action, CancellationToken cancellationToken)
    {
        if (applicationDbContext.Database.CurrentTransaction is not null)
            return await action();

        T result;

        try
        {
            await using var transaction = await applicationDbContext.Database.BeginTransactionAsync(cancellationToken);

            result = await action();

            // Not the request token: a cancelled client must not leave a half-committed change.
            await transaction.CommitAsync(CancellationToken.None);
        }
        catch
        {
            _afterCommit.Clear();
            throw;
        }

        await RunAfterCommit();

        return result;
    }

    public async Task AfterCommit(Func<Task> action)
    {
        if (applicationDbContext.Database.CurrentTransaction is null)
        {
            await Run(action);
            return;
        }

        _afterCommit.Add(action);
    }

    private async Task RunAfterCommit()
    {
        var actions = _afterCommit.ToList();
        _afterCommit.Clear();

        foreach (var action in actions)
            await Run(action);
    }

    private async Task Run(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An action after commit failed");
        }
    }
}
