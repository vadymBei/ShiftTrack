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
    /// Token from <c>HttpContext.RequestAborted</c>.
    /// It is passed automatically through <see cref="ICancellationTokenProvider"/>,
    /// so explicitly passing it to <c>Mediator.Send()</c> is no longer required.
    /// Use this only if you want to explicitly override the token.
    /// </summary>
    protected CancellationToken CancellationToken => HttpContext.RequestAborted;
}