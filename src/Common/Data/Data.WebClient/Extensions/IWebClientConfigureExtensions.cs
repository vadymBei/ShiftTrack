using Data.WebClient.Enums;
using Data.WebClient.Interfaces;
using Data.WebClient.Models;
using Data.WebClient.Options;
using Kernel.Exceptions;

namespace Data.WebClient.Extensions
{
    public static class IWebClientConfigureExtensions
    {
        /// <summary>
        /// Initializing default settings for WebClient
        /// </summary>
        public static IWebClient Configure(this IWebClient webClient, Action<IWebClientConfiguration> baseConfiguration)
        {
            // get configuration
            var cnfg = new WebClientConfiguration();
            baseConfiguration.Invoke(cnfg);

            // recreate httpClient with handler if ssl errors is true
            if (cnfg.IgnoreSslErrors)
                webClient.SetIgnoreSslErrors();

            // configure web resource
            if (!string.IsNullOrEmpty(cnfg.WebResourcePath))
                webClient.WithWebResource(cnfg.WebResourcePath);

            // configure request uri
            if (!string.IsNullOrEmpty(cnfg.RequestUri))
                webClient.WithUri(cnfg.RequestUri);

            // configure errors handling mode
            webClient.Configuration.ErrorHandlingMode = cnfg.ErrorHandlingMode;

            // set default authentication type
            webClient.Configuration.AuthenticationType = cnfg.AuthenticationType;

            switch (webClient.Configuration.AuthenticationType)
            {
                case AuthenticationType.Basic:
                    webClient.WithBasicAuthentication();
                    break;

                case AuthenticationType.Bearer:
                    webClient.WithBearerAuthentication();
                    break;
            }

            return webClient;
        }

        /// <summary>
        /// Recreating http client is allowed only when initializing settings
        /// </summary>
        private static IWebClient SetIgnoreSslErrors(this IWebClient webClient)
        {
            webClient.Configuration.IgnoreSslErrors = true;

            var engine = webClient as IWebClientEngine;
            engine.SetNoSslValidation();

            return webClient;
        }

        /// <summary>
        /// Use web resource from appsettings section in format "WebResourceName/SegmentName"
        /// </summary>
        /// <param name="webResourcePath">WebResourceName/SegmentName - web resource path</param>
        public static IWebClient WithWebResource(this IWebClient webClient, string webResourcePath)
        {
            var engine = webClient as IWebClientEngine;
            engine.SetWebResource(webResourcePath);

            return webClient;
        }

        /// <summary>
        /// Use programmable WebResource. With first segment in collection
        /// </summary>
        public static IWebClient WithWebResource(this IWebClient webClient, WebResource webResource)
        {
            var segment = webResource.Segments.FirstOrDefault();

            webClient.Configuration.WebResource = webResource ??
                throw new NotFoundException($"Resource not found in resources collection");

            webClient.Configuration.Segment = segment ??
                throw new NotFoundException($"No segment in resource {webResource.Name} segments collection");

            webClient.Configuration.RequestUri = webClient.Configuration.WebResource.Host + webClient.Configuration.Segment.Url;

            return webClient;
        }

        /// <summary>
        /// Replace WebResourceName/SegmentName to custom uri string
        /// </summary>
        /// <param name="url">Path uri</param>
        public static IWebClient WithUri(this IWebClient webClient, string uri)
        {
            webClient.Configuration.WebResource = null;
            webClient.Configuration.Segment = null;
            webClient.Configuration.WebResourcePath = string.Empty;

            webClient.Configuration.RequestUri = uri;

            return webClient;
        }

        /// <summary>
        /// Set error handling mode for request
        /// </summary>
        public static IWebClient WithErrorHandlingMode(this IWebClient webClient, ErrorHandlingMode mode)
        {
            // configure errors handling mode
            webClient.Configuration.ErrorHandlingMode = mode;

            return webClient;
        }
    }
}
