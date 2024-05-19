using Kernel.Constants;
using System.Net;

namespace Kernel.Exceptions
{
    public class KernelException : Exception
    {
        public HttpStatusCode Code { get; private set; }
        public string ErrorMessage { get; private set; }
        public string ErrorType { get; private set; }

        public KernelException(HttpStatusCode code, string errorMessage, string errorType = BaseErrorType.KernelError)
        {
            Code = code;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }
    }
}
