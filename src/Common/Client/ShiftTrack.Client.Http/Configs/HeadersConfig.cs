namespace ShiftTrack.Client.Http.Configs;

public class HeadersConfig
{
    public Dictionary<string, string> HeadersCollection { get; set; } = new Dictionary<string, string>();

    protected internal void SetOrReplaceHeader(string key, string value)
    {
        if (HeadersCollection.Any(x => x.Key == key))
            HeadersCollection.Remove(key);

        HeadersCollection.Add(key, value);
    }
}