using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Infrastructure.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Common.Services;

public class UnitOfWork(
    IApplicationDbContext applicationDbContext) : IUnitOfWork
{
    public async Task<T> ExecuteInTransaction<T>(Func<Task<T>> action, CancellationToken cancellationToken)
    {
        if (applicationDbContext.Database.CurrentTransaction is not null)
            return await action();

        await using var transaction = await applicationDbContext.Database.BeginTransactionAsync(cancellationToken);

        var result = await action();

        // Not the request token: a cancelled client must not leave a half-committed change.
        await transaction.CommitAsync(CancellationToken.None);

        return result;
    }
}
