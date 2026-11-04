using ShiftTrack.Application.Modules.Communication.Messenger.Constants;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Services;

/// <summary>A fixed window per employee, kept in memory of the instance: protects from a stuck client or a script.</summary>
public class MessageRateLimiter(IMemoryCache memoryCache) : IMessageRateLimiter
{
    private sealed class Window
    {
        public int Count;
    }

    public bool TryAcquire(long employeeId)
    {
        var window = memoryCache.GetOrCreate($"messenger:rate:{employeeId}", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = MessengerConstants.RateLimitWindow;
            return new Window();
        })!;

        return Interlocked.Increment(ref window.Count) <= MessengerConstants.RateLimitMessages;
    }
}
