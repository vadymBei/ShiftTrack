using ShiftTrack.Kernel.Exceptions;
using System.Net;

namespace User.Authentication.Core.Application.Common.Exceptions;

public class CreateUserRoleException(string details) : CustomException(
    HttpStatusCode.BadRequest,
    $"Failed to create user role. {details}.",
    "AUTH_CREATE_USER_ROLE_ERROR");