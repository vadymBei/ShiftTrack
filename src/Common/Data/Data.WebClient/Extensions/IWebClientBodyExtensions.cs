using Data.WebClient.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Data.WebClient.Extensions
{
    public static class IWebClientBodyExtensions
    {
        /// <summary>
        /// Add content body to request as string
        /// </summary>
        public static IWebClient WithStringContent(this IWebClient webClient, string bodyContent)
        {
            var data = new StringContent(bodyContent, Encoding.UTF8, "application/json");

            webClient.Configuration.HttpContent = data;

            return webClient;
        }

        /// <summary>
        /// Add content body to request as string with Json serialization
        /// </summary>
        public static IWebClient WithStringContent(this IWebClient webClient, object bodyContentObject)
        {
            // request payload
            var json = JsonConvert.SerializeObject(bodyContentObject);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            webClient.Configuration.HttpContent = data;

            return webClient;
        }

        /// <summary>
        /// Add content body to request as string with Json serialization
        /// </summary>
        public static IWebClient WithStringContent(this IWebClient webClient, object bodyContentObject, Encoding encoding, string mediaType = "application/json")
        {
            // request payload
            var json = JsonConvert.SerializeObject(bodyContentObject);
            var data = new StringContent(json, encoding, mediaType);

            webClient.Configuration.HttpContent = data;

            return webClient;
        }
    }
}
