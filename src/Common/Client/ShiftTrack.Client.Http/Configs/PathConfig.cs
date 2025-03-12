using ShiftTrack.Client.Extensions;
using ShiftTrack.Client.Options;

namespace ShiftTrack.Client.Http.Configs;

public class PathConfig
{
    public string Uri { get; set; }
    private IEnumerable<Resource> Resources { get; set; }
    public Resource Resource { get; set; }
    public Segment Segment { get; set; }

    public PathConfig(
        IEnumerable<Resource> resources)
    {
        Resources = resources;
    }
        
    protected internal void SetUri(string pattern)
    {
        if (pattern.Contains("http://") || pattern.Contains("https://"))
        {
            Uri = pattern;
        }
        else
        {
            Resource = Resources.FindResource(pattern);
            Segment = Resource.FindSegment(pattern);
            
            Uri = Resource.Uri + Segment.Path;
        }
    }
}