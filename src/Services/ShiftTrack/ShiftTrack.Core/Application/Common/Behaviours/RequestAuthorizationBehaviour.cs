using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Common.Behaviours;

public class RequestAuthorizationBehaviour<TRequest, TResponse>(
    IHttpContextAccessor httpContextAccessor,
    IEmployeeService employeeService,
    ICurrentUserService currentUserService)
    : IPipelineBehaviour<TRequest, TResponse>
    where TRequest : IRequest<TResponse>

{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var employeeId = httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(employeeId))
        {
            var employee = await employeeService.GetByIntegrationId(employeeId, cancellationToken);

            if (employee is not null)
            {
                currentUserService.Employee = employee;
            }
        }

        return await next();
    }
}

public class RequestAuthorizationBehaviour<TRequest>(
    IHttpContextAccessor httpContextAccessor,
    IEmployeeService employeeService,
    ICurrentUserService currentUserService)
    : IPipelineBehaviour<TRequest>
{
    public async Task Handle(
        TRequest request,
        RequestHandlerDelegate next,
        CancellationToken cancellationToken)
    {
        var employeeId = httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(employeeId))
        {
            var employee = await employeeService.GetByIntegrationId(employeeId, cancellationToken);

            if (employee is not null)
            {
                currentUserService.Employee = employee;
            }
        }

        await next();
    }
}