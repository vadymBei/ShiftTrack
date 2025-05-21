using ShiftTrack.Kernel.Exceptions;
using System.Net;

namespace User.Authentication.Core.Application.Common.Exceptions;

public class UpdateUserException(string details) : CustomException(
    HttpStatusCode.BadRequest,
    $"Failed to update user. {details}.",
    "AUTH_UPDATE_USER_ERROR");