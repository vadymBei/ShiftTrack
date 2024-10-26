using Duende.IdentityServer.Models;
using IdentityModel;

namespace User.Authentication.Core.Infrastructure.OAuth.Clients
{
    public static class ShiftTrackClient
    {
        public static Client Register()
        {
            var grantsCollection = new List<string>(GrantTypes.ResourceOwnerPasswordAndClientCredentials);

            var client = new Client
            {
                ClientId = "shift-track-client",
                ClientName = "Shift track client",

                AllowedGrantTypes = grantsCollection,
                RequireClientSecret = true,
                ClientSecrets = { new Secret("d9c1a730-7337-4f64-9f6a-d4b35d7282c8".Sha256()) },

                AllowedScopes = {
                    "shift-track-api",
                    "openid",
                    "offline_access",
                    JwtClaimTypes.Profile,
                    JwtClaimTypes.Role,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.Id
                },

                RequireConsent = false,
                AlwaysIncludeUserClaimsInIdToken = true,
                IncludeJwtId = true,
                AlwaysSendClientClaims = true,
                AccessTokenLifetime = 300,

                //refresh token settings
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AbsoluteRefreshTokenLifetime = int.MaxValue,
                SlidingRefreshTokenLifetime = int.MaxValue,
                UpdateAccessTokenClaimsOnRefresh = true
            };

            return client;
        }
    }
}
