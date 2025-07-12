using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace User.Authentication.Application.Features.oAuth.Common.Exceptions;

public class UpdateUserException(string details) : CustomException(
    HttpStatusCode.BadRequest,
    $"Failed to update user. {details}.",
    "AUTH_UPDATE_USER_ERROR");