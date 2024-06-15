using ShiftTrack.WebClient.Exceptions;
using ShiftTrack.WebClient.Options;

namespace ShiftTrack.WebClient.Extensions
{
    public static class ResourceExtensions
    {
        public static Resource FindResource(this List<Resource> resources, string pattern)
        {
            var resourcePathSplitted = pattern.Split('/');

            if (resourcePathSplitted.Length != 2)
            {
                throw new PathPatternException(pattern);
            }

            var resource = resources
                .SingleOrDefault(c => c.Name == resourcePathSplitted[0]);

            if (resource == null)
            {
                throw new ResourceNotFoundException(resourcePathSplitted[0]);
            }

            return resource;
        }

        public static Segment FindSegment(this Resource resource, string pattern)
        {
            var resourcePathSplitted = pattern.Split('/');

            if (resourcePathSplitted.Length != 2)
            {
                throw new PathPatternException(pattern);
            }

            var segment = resource.Segments
                .SingleOrDefault(c => c.Name == resourcePathSplitted[1]);

            if (segment == null)
            {
                throw new SegmentNotFoundException(resourcePathSplitted[1], resourcePathSplitted[0]);
            }

            return segment;
        }
    }
}
