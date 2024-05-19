using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ShiftTrack.Core.Infrastructure.Interceptors
{
    public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData is null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            IEnumerable<EntityEntry<ISoftDeletable>> entries = eventData
                .Context
                .ChangeTracker
                .Entries<ISoftDeletable>()
                .Where(x => x.State == EntityState.Deleted)
                .ToList();

            if (entries.Any())
            {
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.Now;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
