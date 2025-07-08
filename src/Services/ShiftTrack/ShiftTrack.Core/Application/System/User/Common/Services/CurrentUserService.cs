using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Services;

public class CurrentUserService : ICurrentUserService
{
    public Employee Employee { get; set; }
    public List<string> Roles { get; }

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        var userRoles = httpContextAccessor.HttpContext?.User?
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        if (userRoles != null)
            Roles = userRoles;
    }
}