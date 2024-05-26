using Kernel.Constants;
using Kernel.Exceptions;
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
