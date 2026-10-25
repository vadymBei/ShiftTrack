using Microsoft.Extensions.Logging;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Notifications.EventHandlers;

/// <summary>
/// Notifications are a side effect: a failure here is logged and must never fail the business operation
/// that raised the event.
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
        }
    }

    protected abstract Task HandleEvent(TEvent domainEvent, CancellationToken cancellationToken);
}
