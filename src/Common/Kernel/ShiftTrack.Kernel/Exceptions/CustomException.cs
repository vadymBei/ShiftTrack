using ShiftTrack.Kernel.Constants;
using System.Net;

namespace ShiftTrack.Kernel.Exceptions;

public abstract class CustomException(
    HttpStatusCode code,
    string errorMessage,
    string errorType)
    : KernelException(
        code,
        string.IsNullOrEmpty(errorMessage) ? "Kernel custom exception" : errorMessage,
        string.IsNullOrEmpty(errorType) ? BaseErrorType.KernelError : errorType.ToUpper());