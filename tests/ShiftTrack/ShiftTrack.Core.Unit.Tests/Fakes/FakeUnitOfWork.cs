using ShiftTrack.Application.Common.Interfaces;

namespace ShiftTrack.Core.Unit.Tests.Fakes;

/// <summary>
/// Runs the action as if in a transaction and keeps the after-commit actions until it ends,
/// so a test sees what would be sent after the commit — and that nothing is sent when the action fails.
/// </summary>
public sealed class FakeUnitOfWork : IUnitOfWork
{
    private readonly List<Func<Task>> _afterCommit = [];
    private bool _inTransaction;

    public int Transactions { get; private set; }

    public async Task<T> ExecuteInTransaction<T>(Func<Task<T>> action, CancellationToken cancellationToken)
    {
        if (_inTransaction)
            return await action();

        Transactions++;
        _inTransaction = true;

        T result;

        try
        {
            result = await action();
        }
        catch
        {
            _afterCommit.Clear();
            throw;
        }
        finally
        {
            _inTransaction = false;
        }

        foreach (var afterCommit in _afterCommit.ToList())
            await afterCommit();

        _afterCommit.Clear();

        return result;
    }

    public Task AfterCommit(Func<Task> action)
    {
        if (!_inTransaction)
            return action();

        _afterCommit.Add(action);
        return Task.CompletedTask;
    }
}
