using System.Text;

namespace ShiftTrack.Client.Helpers;

public static class AuthTokenHelper
{
    public static string GenerateBasicAuthToken(string username, string password)
    {
        var credentials = $"{username}:{password}";
        var bytes = Encoding.UTF8.GetBytes(credentials);
        var base64Credential = Convert.ToBase64String(bytes);
        
        return $"Basic {base64Credential}";
    }
}