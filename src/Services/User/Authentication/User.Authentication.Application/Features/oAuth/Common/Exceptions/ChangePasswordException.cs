using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace User.Authentication.Application.Features.oAuth.Common.Exceptions;

public class ChangePasswordException(string details) : CustomException(
    HttpStatusCode.BadRequest,
    $"Failed to change password. {details}.",
    "AUTH_CHANGE_PASSWORD_ERROR");