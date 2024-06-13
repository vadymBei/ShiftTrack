using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShiftTrack.WebClient.Http.Options;

namespace ShiftTrack.WebClient.Http.Configuration
{
    public class ClientConfiguration
    {
        public HttpContext HttpContext { get; set; }

        public Auth Auth { get; set; }

        public Body Body { get; set; }

        public Path Path { get; set; }

        public Query Query { get; set; }

        public Headers Headers { get; set; }

        public Timeout Timeout { get; set; }

        public ClientConfiguration(
            IOptions<HttpClientOptions> httpClientOptions,
            IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;

            Auth = new Auth(HttpContext?.Request.Headers["Authorization"].ToString());

            Body = new Body();

            Path = new Path(httpClientOptions.Value.Resources);
           
            Query = new Query();
           
            Headers = new Headers();
            
            Timeout = new Timeout();
        }

        public string GetUri(string path)
        {
            return Path.Uri + path + Query.QueryString;
        }
    }
}
