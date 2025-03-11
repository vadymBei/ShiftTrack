using Newtonsoft.Json;
using ShiftTrack.Client.Http.Configs;
using ShiftTrack.Client.Http.Extensions;
using ShiftTrack.Client.Http.Interfaces;
using ShiftTrack.Client.Http.Models;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Client.Http;

public class Client : IClient
{
    public ClientConfig Configuration { get; }
    public HttpClient HttpClient { get; protected set; }

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
                response.StatusCode,
                await response.Content.ReadAsStringAsync(cancellationToken),
                response);
        }

        return typeof(T) switch
        {
            _ when typeof(T) == typeof(EmptyClientResponce) => null,
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
        await Get<EmptyClientResponce>(path, cancellationToken);
    }

    public async Task Get(CancellationToken cancellationToken)
    {
        await Get<EmptyClientResponce>(null, cancellationToken);
    }
    #endregion

    #region Post

    public async Task<T> Post<T>(string path, CancellationToken cancellationToken) where T : class
    {
        var response = await HttpClient
            .ApplyConfiguration(Configuration)
            .PostAsync(Configuration.GetUri(path), Configuration.Body.HttpContent, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new HttpClientException(
                response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken), response);

        return typeof(T) switch
        {
            _ when typeof(T) == typeof(EmptyClientResponce) => null,
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
        await Post<EmptyClientResponce>(path, cancellationToken);
    }

    public async Task Post(CancellationToken cancellationToken)
    {
        await Post<EmptyClientResponce>(null, cancellationToken);
    }

    #endregion

    #region Put
    public async Task<T> Put<T>(string path, CancellationToken cancellationToken) where T : class
    {
        var response = await HttpClient
            .ApplyConfiguration(Configuration)
            .PutAsync(Configuration.GetUri(path), Configuration.Body.HttpContent, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new HttpClientException(
                response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken), response);

        return typeof(T) switch
        {
            _ when typeof(T) == typeof(EmptyClientResponce) => null,
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
        await Put<EmptyClientResponce>(path, cancellationToken);
    }

    public async Task Put(CancellationToken cancellationToken)
    {
        await Put<EmptyClientResponce>(null, cancellationToken);
    }
    #endregion

    #region Delete
    public async Task<T> Delete<T>(string path, CancellationToken cancellationToken) where T : class
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, Configuration.GetUri(path));

        request.Content = Configuration.Body.HttpContent;

        var response = await HttpClient
            .ApplyConfiguration(Configuration)
            .SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new HttpClientException(
                response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken), response);

        return typeof(T) switch
        {
            _ when typeof(T) == typeof(EmptyClientResponce) => null,
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
        await Delete<EmptyClientResponce>(path, cancellationToken);
    }

    public async Task Delete(CancellationToken cancellationToken)
    {
        await Delete<EmptyClientResponce>(null, cancellationToken);
    }
    #endregion
    
    #region Patch
    public async Task<T> Patch<T>(string path, CancellationToken cancellationToken) where T : class
    {
        var response = await HttpClient
            .ApplyConfiguration(Configuration)
            .PatchAsync(Configuration.GetUri(path), Configuration.Body.HttpContent, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new HttpClientException(
                response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken), response);

        return typeof(T) switch
        {
            _ when typeof(T) == typeof(EmptyClientResponce) => null,
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
        await Patch<EmptyClientResponce>(path, cancellationToken);
    }

    public async Task Patch(CancellationToken cancellationToken)
    {
        await Patch<EmptyClientResponce>(null, cancellationToken);
    }
    #endregion
}