using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShiftTrack.Client.Http.Options;

namespace ShiftTrack.Client.Http.Configs;

public class ClientConfig
{
    public HttpContext HttpContext { get; set; }
    public AuthConfig Auth { get; set; }
    public BodyConfig Body { get; set; }
    public PathConfig Path { get; set; }
    public QueryConfig Query { get; set; }
    public HeadersConfig Headers { get; set; }
    public TimeoutConfig Timeout { get; set; }

    public ClientConfig(
        IOptions<HttpClientOptions> httpClientOptions,
        IHttpContextAccessor httpContextAccessor)
    {
        HttpContext = httpContextAccessor.HttpContext;

        Path = new PathConfig(httpClientOptions.Value.Resources);
        Auth = new AuthConfig(HttpContext?.Request.Headers["Authorization"].FirstOrDefault());
        Body = new BodyConfig();
        Query = new QueryConfig();
        Headers = new HeadersConfig();
        Timeout = new TimeoutConfig();
    }

    public string GetUri(string path)
    {
        return Path.Uri + path + Query.QueryString;
    }
    
}