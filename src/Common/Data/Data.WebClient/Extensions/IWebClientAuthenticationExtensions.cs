using Data.WebClient.Helpers;
using Data.WebClient.Interfaces;
using ShiftTrack.Kernel.Exceptions;
using System.Net.Http.Headers;

namespace Data.WebClient.Extensions
{
    public static class IWebClientAuthenticationExtensions
    {
        /// <summary>
        /// Add basic authentication header from selected WebResource section
        /// </summary>
        public static IWebClient WithBasicAuthentication(this IWebClient webClient)
        {
            if (webClient.Configuration.Segment == null)
                throw new NotFoundException("WebResource segment not specified");

            if (webClient.Configuration.Segment.Login != null)
            {
                var authHeader = new AuthenticationHeaderValue("Basic",
                    AuthHelper.GetBasicAuthToken(webClient.Configuration.Segment.Login, webClient.Configuration.Segment.Password));

                webClient.HttpClient.DefaultRequestHeaders.Authorization = authHeader;
            }

            return webClient;
        }

        /// <summary>
        /// Add basic authentication header with login and password
        /// </summary>
        public static IWebClient WithBasicAuthentication(this IWebClient webClient, string login, string password)
        {
            var authHeader = new AuthenticationHeaderValue("Basic", AuthHelper.GetBasicAuthToken(login, password));

            webClient.HttpClient.DefaultRequestHeaders.Authorization = authHeader;

            return webClient;
        }

        /// <summary>
        /// Add pass-through authentication JWT header
        /// </summary>
        public static IWebClient WithBearerAuthentication(this IWebClient webClient)
        {
            var token = webClient.Configuration.HttpContext?.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token))
                throw new NotFoundException("Bearer authorization token not found in request");

            token = token.Replace("Bearer", "");

            var authHeader = new AuthenticationHeaderValue("Bearer", token.Trim());

            webClient.HttpClient.DefaultRequestHeaders.Authorization = authHeader;

            return webClient;
        }

        /// <summary>
        /// Add authentication Bearer JWT header
        /// </summary>
        public static IWebClient WithBearerAuthentication(this IWebClient webClient, string token)
        {
            var authHeader = new AuthenticationHeaderValue("Bearer", token);

            webClient.HttpClient.DefaultRequestHeaders.Authorization = authHeader;

            return webClient;
        }
    }
}
