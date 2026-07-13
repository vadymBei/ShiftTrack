using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Kernel.CQRS.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator =>
        _mediator ??= base.HttpContext.RequestServices.GetRequiredService<IMediator>();

    /// <summary>
    /// Токен з <c>HttpContext.RequestAborted</c>.
    /// Передається автоматично через <see cref="ICancellationTokenProvider"/> —
    /// явно передавати його в <c>Mediator.Send()</c> більше не потрібно.
    /// Використовуй лише якщо хочеш явно перевизначити токен.
    /// </summary>
    protected CancellationToken CancellationToken => HttpContext.RequestAborted;
}