using Data.WebClient.Interfaces;
using System.Collections.Specialized;
using System.Web;

namespace Data.WebClient.Extensions
{
    public static class IWebClientQueryExtensions
    {
        /// <summary>
        /// Add params to query uri
        /// </summary>
        /// <param name="queryParams">Parameters object - new { key = value}</param>
        public static IWebClient WithQueryParams(this IWebClient webClient, object queryParams)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            // query parameters
            if (queryParams != null)
            {
                if (queryParams is NameValueCollection)
                    return webClient;

                foreach (var property in queryParams.GetType().GetProperties())
                {
                    queryString.Add(property.Name, property.GetValue(queryParams, null).ToString());
                }
            }

            webClient.Configuration.QueryString = "?" + queryString.ToString();

            return webClient;
        }

        /// <summary>
        /// Add params string to query uri
        /// </summary>
        /// <param name="queryParamsString">Query params. Start with '?', like ?key=value</param>
        public static IWebClient WithQueryParams(this IWebClient webClient, string queryParamsString)
        {
            webClient.Configuration.QueryString = queryParamsString;

            return webClient;
        }

        /// <summary>
        /// Add header to request
        /// </summary>
        public static IWebClient WithHeader(this IWebClient webClient, string headerKey, string headerValue)
        {
            webClient.HttpClient.DefaultRequestHeaders.Add(headerKey, headerValue);

            return webClient;
        }
    }
}
