using ShiftTrack.Kernel.Exceptions;
using System.Net;

namespace User.Authentication.Core.Application.Common.Exceptions
{
    public class ChangePasswordException : CustomException
    {
        public ChangePasswordException(string details) : base(
                HttpStatusCode.BadRequest,
                $"Failed to change password. {details}.",
                "AUTH_CHANGE_PASSWORD_ERROR")
        {
            
        }
    }
}
