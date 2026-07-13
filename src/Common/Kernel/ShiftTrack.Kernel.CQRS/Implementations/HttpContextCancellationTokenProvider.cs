using Microsoft.AspNetCore.Http;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Kernel.CQRS.Implementations;

/// <summary>
/// Читає <see cref="CancellationToken"/> з поточного HTTP-запиту.
/// Автоматично скасовується коли клієнт від'єднується або запит завершується.
/// </summary>
internal sealed class HttpContextCancellationTokenProvider(IHttpContextAccessor httpContextAccessor)
    : ICancellationTokenProvider
{
    public CancellationToken Token =>
        httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
}