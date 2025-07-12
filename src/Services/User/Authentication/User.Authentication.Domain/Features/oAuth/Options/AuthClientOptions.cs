namespace User.Authentication.Domain.Features.oAuth.Options;

public class AuthClientOptions
{
    public string Client { get; set; }
    public string Scope { get; set; }
    public string ClientSecret { get; set; }
}