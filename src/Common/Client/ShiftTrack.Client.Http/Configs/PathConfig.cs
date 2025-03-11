using ShiftTrack.Client.Extensions;
using ShiftTrack.Client.Options;

namespace ShiftTrack.Client.Http.Configs;

public class PathConfig(
    IEnumerable<Resource> resources)
{
    public string Uri { get; set; }
    private IEnumerable<Resource> Resources { get; set; } = resources;
    public Resource Resource { get; set; }
    public Segment Segment { get; set; }

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