using Data.WebClient.Enums;
using Data.WebClient.Options;
using Microsoft.AspNetCore.Http;

namespace Data.WebClient.Interfaces
{
    public interface IWebClientConfiguration
    {
        string RequestUri { get; set; }

        public string QueryString { get; set; }

        public string WebResourcePath { get; set; }

        public bool IgnoreSslErrors { get; set; }

        public AuthenticationType AuthenticationType { get; set; }

        public ErrorHandlingMode ErrorHandlingMode { get; set; }

        public WebResource WebResource { get; set; }

        public Segment Segment { get; set; }

        public HttpContent HttpContent { get; set; }

        public HttpContext HttpContext { get; set; }
    }
}
