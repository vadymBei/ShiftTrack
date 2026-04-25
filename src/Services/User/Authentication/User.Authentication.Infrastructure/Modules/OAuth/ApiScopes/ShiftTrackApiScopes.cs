using Duende.IdentityServer.Models;

namespace User.Authentication.Infrastructure.Modules.OAuth.ApiScopes;

public static class ShiftTrackApiScopes
{
    public static IEnumerable<ApiScope> Get()
    {
        return new[]
        {
            new ApiScope("shift-track-api", "Shift track API")
        };
    }
}
