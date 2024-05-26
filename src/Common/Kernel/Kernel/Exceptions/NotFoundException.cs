using Kernel.Constants;
using System.Net;

namespace Kernel.Exceptions
{
    public class NotFoundException : KernelException
    {
        public NotFoundException()
            : base(HttpStatusCode.NotFound,
                  "Kernel exception: Not found",
                  BaseErrorType.NotFoundError)
        {
        }

        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound,
                   message,
                   BaseErrorType.NotFoundError)
        {
        }

        public NotFoundException(string name, object key)
            : base(HttpStatusCode.NotFound,
                   $"Kernel NotFound exception: '{name}' with id '{key}' was not found",
                   BaseErrorType.NotFoundError)
        {
        }
    }
}
