namespace ShiftTrack.Application.Common.Interfaces;

public interface IUnitOfWork
{
    /// <summary>
    /// Runs <paramref name="action"/> in a database transaction and commits it if the action succeeds.
    /// Joins the current transaction if there is one.
    /// </summary>
    Task<T> ExecuteInTransaction<T>(Func<Task<T>> action, CancellationToken cancellationToken);
}
