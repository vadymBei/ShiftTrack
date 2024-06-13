using Data.WebClient.Enums;
using Data.WebClient.Helpers;
using Data.WebClient.Interfaces;
using Data.WebClient.Models;
using Data.WebClient.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ShiftTrack.Kernel.Exceptions;

namespace Data.WebClient.Services
{
    public class WebClientEngine : IWebClient, IWebClientEngine
    {
        public WebClientConfiguration Configuration { get; private set; }
        public HttpClient HttpClient { get; protected set; }


        private readonly WebResourcesOptions _webResourcesOptions;
        private readonly IHttpClientFactory _httpClientFactory;

        public WebClientEngine(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IOptions<WebResourcesOptions> webResourcesOptions)
        {
            _webResourcesOptions = webResourcesOptions.Value;
            _httpClientFactory = httpClientFactory;

            Configuration = new WebClientConfiguration
            {
                ErrorHandlingMode = ErrorHandlingMode.Ignore,
                IgnoreSslErrors = true,
                HttpContext = httpContextAccessor.HttpContext
            };

            HttpClient = _httpClientFactory.CreateClient();
        }

        public void SetWebResource(string webResourcePath)
        {
            var webResourcePathSplitted = webResourcePath.Split('/');

            if (webResourcePathSplitted.Length != 2)
            {
                throw new Exception("WebResourcePath is invalid");
            }

            // get resource settings
            Configuration.WebResource = _webResourcesOptions.Resources
                .FirstOrDefault(c => c.Name == webResourcePathSplitted[0]);

            if (Configuration.WebResource == null)
            {
                throw new NotFoundException($"Resource {webResourcePathSplitted[0]} not found in resources collection");
            }

            // get segment settings
            Configuration.Segment = Configuration.WebResource.Segments
                .FirstOrDefault(c => c.Name == webResourcePathSplitted[1]);

            if (Configuration.Segment == null)
            {
                throw new NotFoundException($"Segment {webResourcePathSplitted[1]} not found in resource {Configuration.WebResource.Name} segments collection");
            }

            Configuration.RequestUri = Configuration.WebResource.Host + Configuration.Segment.Url;
        }

        public void SetNoSslValidation()
        {
            HttpClient = _httpClientFactory.CreateClient("no-ssl-validation");
        }

        public async Task<T> Get<T>(string endpoint, CancellationToken cancellationToken) where T : class
        {
            var url = Configuration.RequestUri + endpoint + Configuration.QueryString;

            var response = await HttpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return ErrorHandlerHelper.HandleError(
                    response, 
                    await response.Content.ReadAsStringAsync(cancellationToken), 
                    Configuration.ErrorHandlingMode) as T;
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

        public async Task Get(string endpoint, CancellationToken cancellationToken)
        {
            await Get<EmptyResponse>(endpoint, cancellationToken);
        }

        public async Task<T> Post<T>(string endpoint, CancellationToken cancellationToken) where T : class
        {
            var url = Configuration.RequestUri + endpoint + Configuration.QueryString;

            var response = await HttpClient.PostAsync(url, Configuration.HttpContent, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return ErrorHandlerHelper.HandleError(
                    response, 
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    Configuration.ErrorHandlingMode) as T;
            }

            return typeof(T) switch
            {
                _ when typeof(T) == typeof(EmptyResponse)=> null,
                _ when typeof(T) == typeof(string) => await response.Content.ReadAsStringAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(Stream) => await response.Content.ReadAsStreamAsync(cancellationToken) as T,
                _ when typeof(T) == typeof(byte[]) => await response.Content.ReadAsByteArrayAsync(cancellationToken) as T,
                _ => JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(cancellationToken))
            };
        }

        public async Task Post(string endpoint, CancellationToken cancellationToken)
        {
            await Post<EmptyResponse>(endpoint, cancellationToken);
        }

        public async Task<T> Put<T>(string endpoint, CancellationToken cancellationToken) where T : class
        {
            var url = Configuration.RequestUri + endpoint + Configuration.QueryString;

            var response = await HttpClient.PutAsync(url, Configuration.HttpContent, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return ErrorHandlerHelper.HandleError(
                    response,
                    await response.Content.ReadAsStringAsync(cancellationToken), 
                    Configuration.ErrorHandlingMode) as T;
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

        public async Task Put(string endpoint, CancellationToken cancellationToken)
        {
            await Put<EmptyResponse>(endpoint, cancellationToken);
        }

        public async Task<T> Delete<T>(string endpoint, CancellationToken cancellationToken) where T : class
        {
            var url = Configuration.RequestUri + endpoint + Configuration.QueryString;

            HttpResponseMessage response;

            if (Configuration.HttpContent == null)
            {
                response = await HttpClient.DeleteAsync(url, cancellationToken);
            }
            else
            {
                response = await HttpClient.SendAsync(
                    new HttpRequestMessage(HttpMethod.Delete, url)
                    {
                        Content = Configuration.HttpContent
                    },
                cancellationToken);
            }

            if (!response.IsSuccessStatusCode)
            {
                return ErrorHandlerHelper.HandleError(
                    response, 
                    await response.Content.ReadAsStringAsync(cancellationToken), 
                    Configuration.ErrorHandlingMode) as T;
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

        public async Task Delete(string endpoint, CancellationToken cancellationToken)
        {
            await Delete<EmptyResponse>(endpoint, cancellationToken);
        }
    }
}
