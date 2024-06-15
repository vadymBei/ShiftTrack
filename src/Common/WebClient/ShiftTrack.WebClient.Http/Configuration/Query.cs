using System.Collections.Specialized;
using System.Web;

namespace ShiftTrack.WebClient.Http.Configuration
{
    public class Query
    {
        public string QueryString { get; set; }

        protected internal void SetQueryParams(object queryParams)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (queryParams != null)
            {
                if (queryParams is string)
                {
                    QueryString = (string)queryParams;
                }

                if (queryParams is NameValueCollection)
                {
                    var query = queryParams as NameValueCollection;

                    var keyValuePair = query.AllKeys
                        .SelectMany(query.GetValues, (key, value) => new
                        {
                            key = key,
                            value = value
                        });

                    foreach (var pair in keyValuePair)
                    {
                        if (pair.value != null)
                        {
                            queryString.Add(pair.key, pair.value);
                        }
                    }

                    return;
                }

                foreach (var property in queryParams.GetType().GetProperties())
                {
                    var propertyValue = property
                        .GetValue(queryParams, null);

                    if (propertyValue == null)
                    {
                        continue;
                    }

                    queryString.Add(property.Name, property.GetValue(queryParams)?.ToString());
                }
            }

            QueryString = "?" + queryString.ToString();
        }

        protected internal void SetQueryParams<T>(string key, IEnumerable<T> queryParams)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            foreach (var queryParam in queryParams)
            {
                if (queryParam == null)
                {
                    continue;
                }

                queryString.Add(key, queryParam.ToString());
            }

            QueryString = "?" + queryString.ToString();
        }
    }
}
