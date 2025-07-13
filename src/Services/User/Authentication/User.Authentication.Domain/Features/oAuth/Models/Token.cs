namespace User.Authentication.Domain.Features.oAuth.Models;

public class Token
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
}