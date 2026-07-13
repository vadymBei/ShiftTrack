namespace ShiftTrack.Kernel.CQRS.Interfaces;

/// <summary>
/// Marker interface for notifications (published to multiple handlers).
/// Unlike <see cref="IRequest"/>, a notification does not return a response
/// and can have zero or more handlers.
/// </summary>
public interface INotification;