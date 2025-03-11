using ShiftTrack.Client.Exceptions;
using ShiftTrack.Client.Options;

namespace ShiftTrack.Client.Extensions;

public static class ResourceExtensions
{
    public static Resource FindResource(this IEnumerable<Resource> resources, string pattern)
    {
        var resourcePathSplit = pattern.Split('/');

        if (resourcePathSplit.Length != 2)
            throw new ResourceNotFoundException(pattern);

        var resource = resources
            .FirstOrDefault(x => x.Name == resourcePathSplit[0]);

        if (resource is null)
            throw new ResourceNotFoundException(resourcePathSplit[0]);

        return resource;
    }
    
    public static Segment FindSegment(this Resource resource, string pattern)
    {
        var resourcePathSplit = pattern.Split('/');

        if (resourcePathSplit.Length != 2)
            throw new PathPatternException(pattern);

        var segment = resource.Segments
            .SingleOrDefault(c => c.Name == resourcePathSplit[1]);
        
        if (segment == null)
            throw new SegmentNotFoundException(resourcePathSplit[1]);

        return segment;
    }
}