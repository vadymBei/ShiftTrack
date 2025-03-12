using System.Net;
using ShiftTrack.Kernel.Constants;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Client.Exceptions;

public class SegmentNotFoundException : KernelException
{
    public SegmentNotFoundException() : base(
            HttpStatusCode.NotFound,
            "ShiftTrack.Client.Http.Exception: segment not found.",
            BaseErrorType.HttpClientError)
    {
        
    }
    
    public SegmentNotFoundException(string segment) : base(
        HttpStatusCode.NotFound,
        $"ShiftTrack.Client.Http.Exception: ({segment}) segment not found.",
        BaseErrorType.HttpClientError)
    {
        
    }
}