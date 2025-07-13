using Duende.IdentityServer.Models;
using IdentityModel;

namespace User.Authentication.Infrastructure.Features.OAuth.ApiResources;

public static class ShiftTrackApi
{
    public static ApiResource Register()
    {
        var api = new ApiResource
        {
            Name = "shift-track-api",
            DisplayName = "Shift track API",
            Description = "Shift track API",
            Enabled = true,
            Scopes = { "shift-track-api" },
            ApiSecrets = { new Secret("d9c1a730-7337-4f64-9f6a-d4b35d7282c8".Sha256()) },
            UserClaims = {
                "openid",
                JwtClaimTypes.Profile,
                JwtClaimTypes.Role,
                JwtClaimTypes.Email,
                JwtClaimTypes.PhoneNumber,
                JwtClaimTypes.Id
            }
        };

        return api;
    }
}