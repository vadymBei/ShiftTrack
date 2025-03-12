using System.Net;
using ShiftTrack.Kernel.Constants;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Client.Exceptions;

public class ResourceNotFoundException : KernelException
{
    public ResourceNotFoundException() : base(
        HttpStatusCode.NotFound,
        "ShiftTrack.Client.Http.Exception: resource not found.",
        BaseErrorType.HttpClientError)
    {
    }
    
    public ResourceNotFoundException(string resource) : base(
        HttpStatusCode.NotFound,
        $"ShiftTrack.Client.Http.Exception: ({resource}) resource not found.",
        BaseErrorType.HttpClientError)
    {
    }
}