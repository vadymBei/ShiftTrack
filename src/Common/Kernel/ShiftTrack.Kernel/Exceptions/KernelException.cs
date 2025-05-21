using ShiftTrack.Kernel.Constants;
using System.Net;

namespace ShiftTrack.Kernel.Exceptions;

public class KernelException(
    HttpStatusCode code,
    string errorMessage,
    string errorType = BaseErrorType.KernelError) : Exception
{
    public HttpStatusCode Code { get; private set; } = code;
    public string ErrorMessage { get; private set; } = errorMessage;
    public string ErrorType { get; private set; } = errorType;
}