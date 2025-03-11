using System.Text;
using Newtonsoft.Json;

namespace ShiftTrack.Client.Http.Configs;

public class BodyConfig
{
    public HttpContent HttpContent { get; set; }

    protected internal void SetBody(HttpContent content)
    {
        HttpContent = content;
    }

    protected internal void SetBody(object content)
    {
        var json = JsonConvert.SerializeObject(content);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        
        HttpContent = data;
    }

    protected internal void SetBody(string jsonContent)
    {
        HttpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
    }

    protected internal void SetBody(KeyValuePair<string, string>[] formData)
    {
        HttpContent = new FormUrlEncodedContent(formData);
    }

    protected internal void SetBody(MultipartFormDataContent formData)
    {
        HttpContent = formData;
    }
}