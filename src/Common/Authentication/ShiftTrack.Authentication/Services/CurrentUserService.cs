using IdentityModel;
using Microsoft.AspNetCore.Http;
using ShiftTrack.Authentication.Interfaces;
using ShiftTrack.Authentication.Models;
using System.Security.Claims;

namespace ShiftTrack.Authentication.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public User User { get; }

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            User = new User
            {
                Id = httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtClaimTypes.Id),
                Login = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier),
                UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name),
                Email = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email),
                PhoneNumber = httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtClaimTypes.PhoneNumber),
                Roles = new List<string>()
            };

            var userRoles = httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

            if (userRoles is not null)
            {
                User.Roles = userRoles;
            }
        }
    }
}
