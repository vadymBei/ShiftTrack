using ShiftTrack.Kernel.Exceptions;
using System.Net;

namespace User.Authentication.Core.Application.Common.Exceptions
{
    public class CreateUserRoleException : CustomException
    {
        public CreateUserRoleException(string details) : base(
                HttpStatusCode.BadRequest,
                $"Failed to create user role. {details}.",
                "AUTH_CREATE_USER_ROLE_ERROR")
        {
        }
    }
}
