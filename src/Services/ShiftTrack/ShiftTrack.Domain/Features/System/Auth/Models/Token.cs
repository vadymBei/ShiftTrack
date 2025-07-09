namespace ShiftTrack.Domain.Features.System.Auth.Models;

public class Token(
    string accessToken, 
    string refreshToken, 
    string tokenType, 
    int expiresIn)
{
    public string AccessToken { get; private set; } = accessToken;
    public string RefreshToken { get; private set; } = refreshToken;
    public string TokenType { get; private set; } = tokenType;
    public int ExpiresIn { get; private set; } = expiresIn;
}