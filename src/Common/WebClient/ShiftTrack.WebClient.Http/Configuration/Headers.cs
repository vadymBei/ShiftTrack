namespace ShiftTrack.WebClient.Http.Configuration
{
    public class Headers
    {
        public Dictionary<string, string> HeadersCollection { get; set; } = new Dictionary<string, string>();

        protected internal void SetOrReplaceHeader(string key, string value)
        {
            if (HeadersCollection.Any(c => c.Key == key))
            {
                HeadersCollection.Remove(key);
            }

            HeadersCollection.Add(key, value);
        }
    }
}
