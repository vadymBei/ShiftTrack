using Kernel.Exceptions;
using System.Net;

namespace User.Authentication.Core.Application.Common.Exceptions
{
    public class CreateUserException : CustomException
    {
        public CreateUserException(string details)
            : base(
                HttpStatusCode.BadRequest,
                $"Failed to register user. {details}.",
                "AUTH_REGISTRATION_ERROR")
        {
        }
    }
}
