using Kernel.Constants;
using Kernel.Exceptions;
using System.Net;

namespace ShiftTrack.WebClient.Exceptions
{
    public class PathPatternException : KernelException
    {
        public PathPatternException() : base(
            HttpStatusCode.NotAcceptable,
            "Sequoia.Client.Http.Exception: invalid path pattern - only urls and web resources in 'Resource/Segment' format are allowed",
            BaseErrorType.WebClientError)
        {
        }

        public PathPatternException(string pattern) : base(
            HttpStatusCode.NotAcceptable,
            $"Sequoia.Client.Http.Exception: invalid path pattern ({pattern}) - only urls and web resources in 'Resource/Segment' format are allowed",
            BaseErrorType.WebClientError)
        {
        }
    }
}
