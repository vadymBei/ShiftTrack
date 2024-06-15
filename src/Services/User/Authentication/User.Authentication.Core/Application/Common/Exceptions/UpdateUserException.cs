using ShiftTrack.Kernel.Exceptions;
using System.Net;

namespace User.Authentication.Core.Application.Common.Exceptions
{
    public class UpdateUserException : CustomException
    {
        public UpdateUserException(string details)
            : base(
                HttpStatusCode.BadRequest,
                $"Failed to update user. {details}.",
                "AUTH_REGISTRATION_ERROR")
        {
        }
    }
}
