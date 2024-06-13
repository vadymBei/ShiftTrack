using ShiftTrack.WebClient.Http.Configuration;

namespace ShiftTrack.WebClient.Http.Extensions
{
    public static class HttpClientExtensions
    {
        private const int DefaultTimeoutInSeconds = 100;

        public static HttpClient ApplyConfiguration(this HttpClient httpClient, ClientConfiguration configuration)
        {
            // headers
            if (configuration.Headers.HeadersCollection.Count > 0)
            {
                foreach (var header in configuration.Headers.HeadersCollection)
                {
                    if (httpClient.DefaultRequestHeaders.Any(c => c.Key == header.Key))
                    {
                        httpClient.DefaultRequestHeaders.Remove(header.Key);
                    }

                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            // auth
            if (!string.IsNullOrEmpty(configuration.Auth.Token))
            {
                const string key = "Authorization";

                if (httpClient.DefaultRequestHeaders.Any(c => c.Key == key))
                {
                    httpClient.DefaultRequestHeaders.Remove(key);
                }

                httpClient.DefaultRequestHeaders.Add(key, configuration.Auth.Token);
            }

            // timeout
            if (configuration.Timeout.TimeoutInSeconds > 0 
                && httpClient.Timeout.TotalSeconds == DefaultTimeoutInSeconds)
            {
                httpClient.Timeout = TimeSpan.FromSeconds(configuration.Timeout.TimeoutInSeconds);
            }

            return httpClient;
        }
    }
}
