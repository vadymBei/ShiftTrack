using System.Net;
using ShiftTrack.Kernel.Constants;

namespace ShiftTrack.Kernel.Exceptions;

public class HttpClientException : KernelException
{
    public HttpResponseMessage HttpResponseMessage { get; set; }

    public HttpClientException(
        HttpStatusCode code, 
        string content, 
        HttpResponseMessage raw) : base(
            code,
            $"CBA.Client.Http.Exception [{raw.StatusCode}]: {(string.IsNullOrWhiteSpace(content) ? "no content" : content)}",
            BaseErrorType.WebClientError)
    {
        HttpResponseMessage = raw;
    }
}