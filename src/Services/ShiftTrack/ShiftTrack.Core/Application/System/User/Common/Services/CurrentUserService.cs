using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly string EmployeeIntegrationId;
    public Employee Employee { get; }
    public List<string> Roles { get; } = [];

    public CurrentUserService(
        IApplicationDbContext applicationDbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        EmployeeIntegrationId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(EmployeeIntegrationId))
        {
            Employee = applicationDbContext.Employees
                .AsNoTracking()
                .Include(x => x.Position)
                .Include(x => x.Department)
                .ThenInclude(x => x.Unit)
                .Include(x => x.EmployeeRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefault(x => x.IntegrationId == EmployeeIntegrationId);

            Roles = Employee?.EmployeeRoles
                .Select(x => x.Role.Name)
                .ToList();
        }
    }
}