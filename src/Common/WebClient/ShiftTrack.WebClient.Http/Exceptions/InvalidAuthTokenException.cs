using ShiftTrack.Kernel.Constants;
using ShiftTrack.Kernel.Exceptions;
using System.Net;

namespace ShiftTrack.WebClient.Http.Exceptions
{
    public class InvalidAuthTokenException : KernelException
    {
        public InvalidAuthTokenException() : base(
            HttpStatusCode.NotAcceptable,
            "CBA.Client.Http.Exception: invalid token - token must be Basic or Bearer",
            BaseErrorType.WebClientError)
        {
        }
    }
}
