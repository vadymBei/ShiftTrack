using ShiftTrack.Kernel.Constants;
using ShiftTrack.Kernel.Exceptions;
using System.Net;

namespace ShiftTrack.WebClient.Exceptions
{
    public class SegmentNotFoundException : KernelException
    {
        public SegmentNotFoundException() : base(
            HttpStatusCode.NotFound,
            "Sequoia.Client.Http.Exception: segment not found",
            BaseErrorType.NotFoundError)
        {
        }

        public SegmentNotFoundException(string name, string resource) : base(
            HttpStatusCode.NotFound,
            $"Sequoia.Client.Http.Exception: segment {name} not found in {resource}",
            BaseErrorType.NotFoundError)
        {
        }
    }
}
