using ShiftTrack.WebClient.Extensions;
using ShiftTrack.WebClient.Http.Exceptions;
using ShiftTrack.WebClient.Options;
using System.Text;

namespace ShiftTrack.WebClient.Http.Configuration
{
    public class Auth
    {
        public string Token { get; set; }

        private List<Resource> Resources { get; set; }

        private string CurrentRequestToken { get; set; }

        public Auth(List<Resource> resources, string token)
        {
            Resources = resources;
            CurrentRequestToken = token;
        }

        protected internal void SetToken(string pattern)
        {
            if (pattern.Contains("Bearer") || pattern.Contains("Basic"))
            {
                Token = pattern;
            }
            else
            {
                throw new InvalidAuthTokenException();
            }
        }

        protected internal void SetToken()
        {
            Token = CurrentRequestToken;
        }

        protected internal void SetBasicToken(string pattern)
        {
            var resource = Resources.FindResource(pattern);

            var segment = resource.FindSegment(pattern);

            if (string.IsNullOrEmpty(segment.Login)
                || string.IsNullOrEmpty(segment.Password))
            {
                throw new InvalidAuthTokenException();
            }

            var byteArray = Encoding.ASCII.GetBytes($"{segment.Login}:{segment.Password}");

            var token = "Basic" + " " + Convert.ToBase64String(byteArray);

            SetToken(token);
        }
    }
}
