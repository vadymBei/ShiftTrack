using ShiftTrack.WebClient.Http.Exceptions;

namespace ShiftTrack.WebClient.Http.Configuration
{
    public class Auth
    {
        public string Token { get; set; }

        private string CurrentRequestToken { get; set; }

        public Auth(string token)
        {
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
    }
}
