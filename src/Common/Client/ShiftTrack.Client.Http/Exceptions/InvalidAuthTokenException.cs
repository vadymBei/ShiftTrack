using System.Net;
using ShiftTrack.Kernel.Constants;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Client.Http.Exceptions;

public class InvalidAuthTokenException : KernelException
{
    public InvalidAuthTokenException() : base(
        HttpStatusCode.NotAcceptable,
        "ShiftTrack.Client.Http.Exception: invalid token. Token have to be Basic or Bearer",
        BaseErrorType.HttpClientError)
    {
        
    }
}