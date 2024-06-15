using ShiftTrack.Kernel.Constants;
using System.Net;

namespace ShiftTrack.Kernel.Exceptions
{
    public abstract class CustomException : KernelException
    {
        public CustomException(HttpStatusCode code, string errorMessage, string errorType)
           : base(code,
                 (string.IsNullOrEmpty(errorMessage)) ? "Kernel custom exception" : errorMessage,
                 (string.IsNullOrEmpty(errorType)) ? BaseErrorType.KernelError : errorType.ToUpper())
        {
        }
    }
}
