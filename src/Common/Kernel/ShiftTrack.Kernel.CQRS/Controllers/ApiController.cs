using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Kernel.CQRS.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    private IMediator _mediator;
    
    protected IMediator Mediator => 
        _mediator ?? (_mediator = base.HttpContext.RequestServices.GetService<IMediator>());
}