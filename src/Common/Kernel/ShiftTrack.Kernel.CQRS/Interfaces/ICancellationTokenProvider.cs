namespace ShiftTrack.Kernel.CQRS.Interfaces;

/// <summary>
/// Надає поточний <see cref="CancellationToken"/> для використання в Mediator.
/// В контексті HTTP — це <c>HttpContext.RequestAborted</c>, 
/// який ASP.NET Core скасовує при від'єднанні клієнта.
/// </summary>
public interface ICancellationTokenProvider
{
    CancellationToken Token { get; }
}