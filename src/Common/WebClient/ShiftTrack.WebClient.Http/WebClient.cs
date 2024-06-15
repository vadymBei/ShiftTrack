using Newtonsoft.Json;
using ShiftTrack.WebClient.Http.Configuration;
using ShiftTrack.WebClient.Http.Exceptions;
using ShiftTrack.WebClient.Http.Extensions;
using ShiftTrack.WebClient.Http.Interfaces;
using ShiftTrack.WebClient.Http.Models;

namespace ShiftTrack.WebClient.Http
{
    public class WebClient : IWebClient
    {
        public ClientConfiguration Configuration { get; protected set; }

        public HttpClient HttpClient { get; protected set; }

        public WebClient(
            ClientConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateClient();
            Configuration = configuration;
        }

        public HttpClient GetHttpClient()
        {
            return HttpClient.ApplyConfiguration(Configuration);
        }

        #region Get

        public async Task<T> Get<T>(string path, CancellationToken cancellationToken) where T : class
        {
            var response = await HttpClient
                .ApplyConfiguration(Configuration)
                .GetAsync(Configuration.GetUri(path), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpClientException(
                    response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken), response);
            }

            return typeof(T) switch
            {
                _ when typeof(T) == typeof(EmptyResponse) => null,
                _ when typeof(T) == typeof(string) => await response.Content.ReadAsStringAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(Stream) => await response.Content.ReadAsStreamAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(byte[]) => await response.Content.ReadAsByteArrayAsync(cancellationToken) as T,
                _ => JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(cancellationToken))
            };
        }

        public async Task<T> Get<T>(CancellationToken cancellationToken) where T : class
        {
            return await Get<T>(null, cancellationToken);
        }

        public async Task Get(string path, CancellationToken cancellationToken)
        {
            await Get<EmptyResponse>(path, cancellationToken);
        }

        public async Task Get(CancellationToken cancellationToken)
        {
            await Get<EmptyResponse>(null, cancellationToken);
        }

        #endregion

        #region Post

        public async Task<T> Post<T>(string path, CancellationToken cancellationToken) where T : class
        {
            var response = await HttpClient
                .ApplyConfiguration(Configuration)
                .PostAsync(Configuration.GetUri(path), Configuration.Body.HttpContent, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpClientException(
                    response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken), response);
            }

            return typeof(T) switch
            {
                _ when typeof(T) == typeof(EmptyResponse) => null,
                _ when typeof(T) == typeof(string) => await response.Content.ReadAsStringAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(Stream) => await response.Content.ReadAsStreamAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(byte[]) => await response.Content.ReadAsByteArrayAsync(cancellationToken) as T,
                _ => JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(cancellationToken))
            };
        }

        public async Task<T> Post<T>(CancellationToken cancellationToken) where T : class
        {
            return await Post<T>(null, cancellationToken);
        }

        public async Task Post(string path, CancellationToken cancellationToken)
        {
            await Post<EmptyResponse>(path, cancellationToken);
        }

        public async Task Post(CancellationToken cancellationToken)
        {
            await Post<EmptyResponse>(null, cancellationToken);
        }

        #endregion

        #region Put

        public async Task<T> Put<T>(string path, CancellationToken cancellationToken) where T : class
        {
            var response = await HttpClient
                .ApplyConfiguration(Configuration)
                .PutAsync(Configuration.GetUri(path), Configuration.Body.HttpContent, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpClientException(
                    response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken), response);
            }

            return typeof(T) switch
            {
                _ when typeof(T) == typeof(EmptyResponse) => null,
                _ when typeof(T) == typeof(string) => await response.Content.ReadAsStringAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(Stream) => await response.Content.ReadAsStreamAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(byte[]) => await response.Content.ReadAsByteArrayAsync(cancellationToken) as T,
                _ => JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(cancellationToken))
            };
        }

        public async Task<T> Put<T>(CancellationToken cancellationToken) where T : class
        {
            return await Put<T>(null, cancellationToken);
        }

        public async Task Put(string path, CancellationToken cancellationToken)
        {
            await Put<EmptyResponse>(path, cancellationToken);
        }

        public async Task Put(CancellationToken cancellationToken)
        {
            await Put<EmptyResponse>(null, cancellationToken);
        }

        #endregion

        #region Delete

        public async Task<T> Delete<T>(string path, CancellationToken cancellationToken) where T : class
        {
            var response = await HttpClient
                .ApplyConfiguration(Configuration)
                .GetAsync(Configuration.GetUri(path), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpClientException(
                    response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken), response);
            }

            return typeof(T) switch
            {
                _ when typeof(T) == typeof(EmptyResponse) => null,
                _ when typeof(T) == typeof(string) => await response.Content.ReadAsStringAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(Stream) => await response.Content.ReadAsStreamAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(byte[]) => await response.Content.ReadAsByteArrayAsync(cancellationToken) as T,
                _ => JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(cancellationToken))
            };
        }

        public async Task<T> Delete<T>(CancellationToken cancellationToken) where T : class
        {
            return await Delete<T>(null, cancellationToken);
        }

        public async Task Delete(string path, CancellationToken cancellationToken)
        {
            await Delete<EmptyResponse>(path, cancellationToken);
        }

        public async Task Delete(CancellationToken cancellationToken)
        {
            await Delete<EmptyResponse>(null, cancellationToken);
        }

        #endregion

        #region Patch

        public async Task<T> Patch<T>(string path, CancellationToken cancellationToken) where T : class
        {
            var response = await HttpClient
                .ApplyConfiguration(Configuration)
                .PatchAsync(Configuration.GetUri(path), Configuration.Body.HttpContent, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpClientException(
                    response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken), response);
            }

            return typeof(T) switch
            {
                _ when typeof(T) == typeof(EmptyResponse) => null,
                _ when typeof(T) == typeof(string) => await response.Content.ReadAsStringAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(Stream) => await response.Content.ReadAsStreamAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(byte[]) => await response.Content.ReadAsByteArrayAsync(cancellationToken) as T,
                _ => JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(cancellationToken))
            };
        }

        public async Task<T> Patch<T>(CancellationToken cancellationToken) where T : class
        {
            return await Patch<T>(null, cancellationToken);
        }

        public async Task Patch(string path, CancellationToken cancellationToken)
        {
            await Patch<EmptyResponse>(path, cancellationToken);
        }

        public async Task Patch(CancellationToken cancellationToken)
        {
            await Patch<EmptyResponse>(null, cancellationToken);
        }

        #endregion
    }
}
