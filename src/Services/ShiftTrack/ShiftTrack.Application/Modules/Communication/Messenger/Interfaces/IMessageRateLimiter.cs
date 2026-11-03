namespace ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;

public interface IMessageRateLimiter
{
    /// <summary>Counts a message of the employee; false when they send too fast.</summary>
    bool TryAcquire(long employeeId);
}
