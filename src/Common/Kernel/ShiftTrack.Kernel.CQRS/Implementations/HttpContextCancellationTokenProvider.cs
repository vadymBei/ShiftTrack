using Microsoft.AspNetCore.Http;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Kernel.CQRS.Implementations;

/// <summary>
/// Reads the <see cref="CancellationToken"/> from the current HTTP request.
/// It is automatically canceled when the client disconnects or the request completes.
/// </summary>
internal sealed class HttpContextCancellationTokenProvider(IHttpContextAccessor httpContextAccessor)
    : ICancellationTokenProvider
{
    public CancellationToken Token =>
        httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
}