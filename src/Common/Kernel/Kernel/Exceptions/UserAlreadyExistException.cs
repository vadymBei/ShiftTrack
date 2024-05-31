using Kernel.Constants;
using System.Net;

namespace Kernel.Exceptions
{
    public class UserAlreadyExistException : KernelException
    {
        public UserAlreadyExistException(string phoneNumber)
            : base(
                  HttpStatusCode.Forbidden,
                  $"User already exist with phone number: {phoneNumber}",
                  BaseErrorType.EntityAlreadyExistError)
        {
        }
    }
}
