using ShiftTrack.WebClient.Http.Interfaces;

namespace ShiftTrack.WebClient.Http.Extensions
{
    public static class WebClientExtensions
    {
        public static IWebClient Path(this IWebClient webClient, string uri)
        {
            webClient.Configuration.Path.SetUri(uri);

            return webClient;
        }

        public static IWebClient Query(this IWebClient webClient, object queryParams)
        {
            webClient.Configuration.Query.SetQueryParams(queryParams);

            return webClient;
        }

        public static IWebClient Query<T>(this IWebClient webClient, string key, IEnumerable<T> queryParams)
        {
            webClient.Configuration.Query.SetQueryParams(key, queryParams);

            return webClient;
        }

        public static IWebClient Headers(this IWebClient webClient, string key, string value)
        {
            webClient.Configuration.Headers.SetOrReplaceHeader(key, value);

            return webClient;
        }

        public static IWebClient Body(this IWebClient webClient, object bodyContent)
        {
            webClient.Configuration.Body.SetBody(bodyContent);

            return webClient;
        }

        public static IWebClient Body(this IWebClient webClient, KeyValuePair<string, string>[] formData)
        {
            webClient.Configuration.Body.SetBody(formData);

            return webClient;
        }

        public static IWebClient Body(this IWebClient webClient, MultipartFormDataContent formData)
        {
            webClient.Configuration.Body.SetBody(formData);

            return webClient;
        }

        public static IWebClient Auth(this IWebClient webClient, string token)
        {
            webClient.Configuration.Auth.SetToken(token);

            return webClient;
        }

        public static IWebClient Auth(this IWebClient webClient)
        {
            webClient.Configuration.Auth.SetToken();

            return webClient;
        }

        public static IWebClient Timeout(this IWebClient webClient, int timeoutInSeconds)
        {
            webClient.Configuration.Timeout.SetTimeout(timeoutInSeconds);

            return webClient;
        }
    }
}
