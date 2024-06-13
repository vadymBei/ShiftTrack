using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ShiftTrack.Kernel.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => 
            _mediator ?? (_mediator = base.HttpContext.RequestServices.GetService<IMediator>());
    }
}
