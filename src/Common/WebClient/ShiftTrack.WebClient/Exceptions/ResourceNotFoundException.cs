using Kernel.Constants;
using Kernel.Exceptions;
using System.Net;

namespace ShiftTrack.WebClient.Exceptions
{
    public class ResourceNotFoundException : KernelException
    {
        public ResourceNotFoundException() : base(
            HttpStatusCode.NotFound,
            "Sequoia.Client.Http.Exception: resource not found",
            BaseErrorType.NotFoundError)
        {
        }

        public ResourceNotFoundException(string name) : base(
            HttpStatusCode.NotFound,
            $"Sequoia.Client.Http.Exception: resource {name} not found",
            BaseErrorType.NotFoundError)
        {
        }
    }
}
