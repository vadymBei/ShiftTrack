using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Common.Behaviours;

public class RequestAuthorizationBehaviour<TRequest, TResponse>(
    IHttpContextAccessor httpContextAccessor,
    IApplicationDbContext applicationDbContext,
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
            var employee = await applicationDbContext.Employees
                .AsNoTracking()
                .Include(x => x.Position)
                .Include(x => x.Department)
                .ThenInclude(x => x.Unit)
                .Include(x => x.EmployeeRoles)
                .ThenInclude(x => x.Role)
                .Include(x => x.EmployeeRoles)
                .ThenInclude(x => x.Units)
                .ThenInclude(x => x.Departments)
                .ThenInclude(x => x.Department)
                .FirstOrDefaultAsync(x => x.IntegrationId == employeeId, cancellationToken);
            
            currentUserService.Employee = employee;
        }

        return await next();
    }
}