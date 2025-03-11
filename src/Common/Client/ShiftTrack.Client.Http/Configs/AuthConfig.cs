using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Helpers;
using ShiftTrack.Client.Http.Exceptions;
using ShiftTrack.Client.Options;

namespace ShiftTrack.Client.Http.Configs;

public class AuthConfig
{
    public string Token { get; set; }
    public string CurrentRequestToken { get; set; }

    public AuthConfig(string token)
    {
        CurrentRequestToken = token;
    }

    protected internal void SetToken(string pattern)
    {
        if (pattern.Contains("Basic") || pattern.Contains("Bearer"))
            Token = pattern;
        else
            throw new InvalidAuthTokenException();
    }

    protected internal void SetToken(AuthProvider authProvider, AuthScheme authScheme)
    {
        switch (authProvider)
        {
            case AuthProvider.Basic:
            {
                if (authScheme.Basic == null)
                {
                    throw new InvalidAuthTokenException();
                }

                Token = AuthTokenHelper.GenerateBasicAuthToken(
                    authScheme.Basic.Username,
                    authScheme.Basic.Password);

                break;
            }
            case AuthProvider.Bearer:
                throw new NotImplementedException();

            case AuthProvider.None:
            default:
                throw new ArgumentOutOfRangeException(nameof(authProvider), authProvider, null);
        }
    }

    protected internal void SetToken()
    {
        Token = CurrentRequestToken;
    }
}