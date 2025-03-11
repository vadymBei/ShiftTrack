namespace ShiftTrack.Client.Options;

public class Resource
{
    public string Name { get; set; }
    public string Uri { get; set; }
    public IEnumerable<Segment> Segments { get; set; }
}