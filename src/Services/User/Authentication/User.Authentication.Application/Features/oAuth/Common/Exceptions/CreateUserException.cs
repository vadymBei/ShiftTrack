using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace User.Authentication.Application.Features.oAuth.Common.Exceptions;

public class CreateUserException(string details) : CustomException(
    HttpStatusCode.BadRequest,
    $"Failed to register user. {details}.",
    "AUTH_REGISTRATION_ERROR");