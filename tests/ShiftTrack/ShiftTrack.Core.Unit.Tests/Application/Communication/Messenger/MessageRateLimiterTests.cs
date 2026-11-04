using Microsoft.Extensions.Caching.Memory;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;
using ShiftTrack.Application.Modules.Communication.Messenger.Services;

namespace ShiftTrack.Core.Unit.Tests.Application.Communication.Messenger;

public class MessageRateLimiterTests
{
    private readonly MessageRateLimiter _limiter = new(new MemoryCache(new MemoryCacheOptions()));

    [Fact]
    public void AllowsTheLimit_ThenRejects()
    {
        for (var i = 0; i < MessengerConstants.RateLimitMessages; i++)
            Assert.True(_limiter.TryAcquire(1));

        Assert.False(_limiter.TryAcquire(1));
    }

    [Fact]
    public void CountsEveryEmployeeSeparately()
    {
        for (var i = 0; i <= MessengerConstants.RateLimitMessages; i++)
            _limiter.TryAcquire(1);

        Assert.True(_limiter.TryAcquire(2));
    }
}
