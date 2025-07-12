using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace User.Authentication.Application.Features.oAuth.Common.Exceptions;

public class CreateUserRoleException(string details) : CustomException(
    HttpStatusCode.BadRequest,
    $"Failed to create user role. {details}.",
    "AUTH_CREATE_USER_ROLE_ERROR");