using Kernel.Exceptions;
using System.Net;

namespace User.Authentication.Core.Application.Common.Exceptions
{
    public class InvalidCredentialsException : CustomException
    {
        public InvalidCredentialsException(string login, string details)
            : base(
                HttpStatusCode.BadRequest,
                $"Invalid login or password for '{login}'. {details}",
                "AUTH_CREDENTIALS_ERROR")
        {
        }
    }
}
