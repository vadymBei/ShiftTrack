namespace ShiftTrack.Application.Common.Interfaces;

public interface IUnitOfWork
{
    /// <summary>
    /// Runs <paramref name="action"/> in a database transaction and commits it if the action succeeds.
    /// Joins the current transaction if there is one.
    /// </summary>
    Task<T> ExecuteInTransaction<T>(Func<Task<T>> action, CancellationToken cancellationToken);

    /// <summary>
    /// Runs <paramref name="action"/> after the current transaction is committed, or right away without
    /// a transaction. For side effects that must not be seen before the data is saved, like real-time
    /// messages. A failed action is logged and doesn't fail the request; nothing runs if the transaction fails.
    /// </summary>
    Task AfterCommit(Func<Task> action);
}
