namespace ShiftTrack.WebClient.Http.Configuration
{
    public class Timeout
    {
        public int TimeoutInSeconds { get; set; }

        protected internal void SetTimeout(int seconds)
        {
            TimeoutInSeconds = seconds;
        }
    }
}
