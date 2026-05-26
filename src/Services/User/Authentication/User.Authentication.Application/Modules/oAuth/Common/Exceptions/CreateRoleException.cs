using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace User.Authentication.Application.Modules.oAuth.Common.Exceptions;

public class CreateRoleException(string details) : CustomException(
    HttpStatusCode.BadRequest,
    $"Failed to create role. {details}.",
    "AUTH_CREATE_ROLE_ERROR");