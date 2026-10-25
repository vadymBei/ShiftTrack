using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Communication.Notifications.Dtos;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Domain.Modules.Communication.Notifications.Entities;
using ShiftTrack.Domain.Modules.Communication.Notifications.Enums;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.Communication.Notifications.Repositories;

public class NotificationRepository(
    IApplicationDbContext applicationDbContext) : INotificationRepository
{
    public async Task AddRange(IReadOnlyCollection<Notification> notifications, CancellationToken cancellationToken)
    {
        await applicationDbContext.Notifications.AddRangeAsync(notifications, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Notification> GetById(long id, CancellationToken cancellationToken)
    {
        return await applicationDbContext.Notifications
                   .AsNoTracking()
                   .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
               ?? throw new EntityNotFoundException(typeof(Notification), id);
    }

    public async Task<IEnumerable<Notification>> GetForEmployee(long employeeId, NotificationsFilterDto filter,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var query = applicationDbContext.Notifications
            .AsNoTracking()
            .Include(x => x.Author)
            .Where(x => x.EmployeeId == employeeId
                        && (x.ExpiresAt == null || x.ExpiresAt > now));

        if (filter.OnlyUnread)
            query = query.Where(x => x.ReadAt == null);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(filter.Take)
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountUnread(long employeeId, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        return applicationDbContext.Notifications
            .Where(x => x.EmployeeId == employeeId
                        && x.ReadAt == null
                        && (x.ExpiresAt == null || x.ExpiresAt > now))
            .CountAsync(cancellationToken);
    }

    public Task MarkAsRead(long id, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        return applicationDbContext.Notifications
            .Where(x => x.Id == id && x.ReadAt == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.ReadAt, now), cancellationToken);
    }

    public Task<int> MarkAllAsRead(long employeeId, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        return applicationDbContext.Notifications
            .Where(x => x.EmployeeId == employeeId && x.ReadAt == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.ReadAt, now), cancellationToken);
    }

    public async Task<IReadOnlyCollection<long>> MarkAsReadByEntity(
        NotificationModuleType moduleType,
        long entityId,
        IReadOnlyCollection<NotificationType> types,
        CancellationToken cancellationToken)
    {
        var query = applicationDbContext.Notifications
            .Where(x => x.ModuleType == moduleType
                        && x.EntityId == entityId
                        && x.ReadAt == null
                        && types.Contains(x.Type));

        var employeeIds = await query
            .Select(x => x.EmployeeId)
            .Distinct()
            .ToListAsync(cancellationToken);

        if (employeeIds.Count == 0)
            return employeeIds;

        var now = DateTime.UtcNow;

        await query.ExecuteUpdateAsync(setters => setters.SetProperty(x => x.ReadAt, now), cancellationToken);

        return employeeIds;
    }

    public Task Delete(long id, CancellationToken cancellationToken)
    {
        return applicationDbContext.Notifications
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
