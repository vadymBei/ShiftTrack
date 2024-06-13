using Newtonsoft.Json;
using System.Text;

namespace ShiftTrack.WebClient.Http.Configuration
{
    public class Body
    {
        public HttpContent HttpContent { get; set; }

        protected internal void SetBody(HttpContent httpContent)
        {
            HttpContent = httpContent;
        }

        protected internal void SetBody(object bodyContent)
        {
            var json = JsonConvert.SerializeObject(bodyContent);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpContent = data;
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
}
