namespace ShiftTrack.Kernel.CQRS.Interfaces;

/// <summary>
/// Provides the current <see cref="CancellationToken"/> for use in Mediator.
/// In an HTTP context, this is <c>HttpContext.RequestAborted</c>,
/// which ASP.NET Core cancels when the client disconnects.
/// </summary>
public interface ICancellationTokenProvider
{
    CancellationToken Token { get; }
}