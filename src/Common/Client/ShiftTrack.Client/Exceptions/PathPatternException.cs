using System.Net;
using ShiftTrack.Kernel.Constants;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Client.Exceptions;

public class PathPatternException : KernelException
{
    public PathPatternException() : base(
        HttpStatusCode.NotAcceptable,
        "ShiftTrack.Client.Http.Exception: invalid path pattern - only urls and web resources in Resource/Segments format are allowed.",
        BaseErrorType.HttpClientError)
    {
    }

    public PathPatternException(string pattern) : base(
        HttpStatusCode.NotAcceptable,
        $"ShiftTrack.Client.Http.Exception: invalid path pattern ({pattern}) - only urls and web resources in Resource/Segments format are allowed.",
        BaseErrorType.HttpClientError)
    {
    }
}