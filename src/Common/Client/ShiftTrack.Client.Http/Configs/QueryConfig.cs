using System.Collections.Specialized;
using System.Web;

namespace ShiftTrack.Client.Http.Configs;

public class QueryConfig
{
    public string QueryString { get; set; }

    protected internal void SetQueryParams(object queryParams)
    {
        if (queryParams == null)
        {
            QueryString = string.Empty;
            return;
        }

        var queryString = HttpUtility.ParseQueryString(string.Empty);

        switch (queryParams)
        {
            case string s:
            {
                QueryString = s;
                return;
            }

            case NameValueCollection collection:
            {
                foreach (string key in collection.AllKeys)
                {
                    foreach (var value in collection.GetValues(key) ?? Array.Empty<string>())
                    {
                        queryString.Add(key, value);
                    }
                }

                QueryString = "?" + queryString;
                return;
            }

            default:
            {
                var properties = queryParams
                    .GetType()
                    .GetProperties();

                foreach (var property in properties)
                {
                    if (property?.GetValue(queryParams) is { } value)
                    {
                        queryString.Add(property.Name, value.ToString());
                    }
                }

                QueryString = "?" + queryString;
                return;
            }
        }
    }

    protected internal void SetQueryParams<T>(string key, IEnumerable<T> queryParams)
    {
        if (string.IsNullOrWhiteSpace(key) 
            || queryParams == null 
            || !queryParams.Any())
        {
            QueryString = string.Empty;
            return;
        }

        var queryString = HttpUtility.ParseQueryString(string.Empty);

        foreach (var queryParam in queryParams.Where(q => q != null))
        {
            queryString.Add(key, queryParam.ToString());
        }

        QueryString = "?" + queryString;
    }
}