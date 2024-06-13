using ShiftTrack.Kernel.Constants;
using ShiftTrack.Kernel.Exceptions;
using System.Net;

namespace Data.WebClient.Exceptions
{
    public class WebClientException : CustomException
    {
        public WebClientException(HttpStatusCode code, string errorMessage)
            : base(code, errorMessage, BaseErrorType.WebClientError)
        {
        }
    }
}
