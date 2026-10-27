using Microsoft.Extensions.Logging;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.EventHandlers;

/// <summary>
/// Handlers run from the outbox in ShiftTrack.Jobs, after the business operation is committed.
/// A failure is logged and rethrown, so the outbox retries the event later.
/// </summary>
public abstract class NotificationEventHandler<TEvent>(ILogger logger) : IDomainEventHandler<TEvent>
    where TEvent : IDomainEvent
{
    public async Task Handle(TEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            await HandleEvent(domainEvent, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to create notifications for {Event}", typeof(TEvent).Name);
            throw;
        }
    }

    protected abstract Task HandleEvent(TEvent domainEvent, CancellationToken cancellationToken);
}
