using ShiftTrack.Client.Enums;
using ShiftTrack.Client.Http.Interfaces;

namespace ShiftTrack.Client.Http.Extensions;

public static class ClientExtensions
{
    public static IClient Path(this IClient client, string uri)
    {
        client.Configuration.Path.SetUri(uri);

        return client;
    }

    public static IClient Query(this IClient client, object queryParams)
    {
        client.Configuration.Query.SetQueryParams(queryParams);

        return client;
    }

    public static IClient Query<T>(this IClient client, string key, IEnumerable<T> queryParams)
    {
        client.Configuration.Query.SetQueryParams(key, queryParams);

        return client;
    }

    public static IClient Headers(this IClient client, string key, string value)
    {
        client.Configuration.Headers.SetOrReplaceHeader(key, value);

        return client;
    }

    public static IClient Body(this IClient client, object bodyContent)
    {
        client.Configuration.Body.SetBody(bodyContent);

        return client;
    }

    public static IClient Body(this IClient client, string bodyContent)
    {
        client.Configuration.Body.SetBody(bodyContent);

        return client;
    }

    public static IClient Body(this IClient client, KeyValuePair<string, string>[] formData)
    {
        client.Configuration.Body.SetBody(formData);

        return client;
    }

    public static IClient Body(this IClient client, MultipartFormDataContent formData)
    {
        client.Configuration.Body.SetBody(formData);

        return client;
    }

    public static IClient Auth(this IClient client, string token)
    {
        client.Configuration.Auth.SetToken(token);

        return client;
    }
    
    public static IClient Auth(this IClient client, AuthProvider provider)
    {
        client.Configuration.Auth.SetToken(provider, client.Configuration.Path.Segment.Auth);

        return client;
    }

    public static IClient Auth(this IClient client)
    {
        client.Configuration.Auth.SetToken();

        return client;
    }

    public static IClient Timeout(this IClient client, int timeoutInSeconds)
    {
        client.Configuration.Timeout.SetTimeout(timeoutInSeconds);

        return client;
    }
}