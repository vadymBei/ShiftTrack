using Duende.IdentityServer.Models;

namespace User.Authentication.Infrastructure.Modules.OAuth.Clients;

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

            AllowedScopes =
            {
                "shift-track-api",
                "offline_access"
            },
            IncludeJwtId = true,
            RequireConsent = false,
            AlwaysIncludeUserClaimsInIdToken = true,
            UpdateAccessTokenClaimsOnRefresh = true,
            AlwaysSendClientClaims = true,
            AccessTokenLifetime = 300,

            //refresh token settings
            AllowOfflineAccess = true
        };

        return client;
    }
}