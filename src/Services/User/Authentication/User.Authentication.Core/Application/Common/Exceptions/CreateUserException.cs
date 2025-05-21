using ShiftTrack.Kernel.Exceptions;
using System.Net;

namespace User.Authentication.Core.Application.Common.Exceptions;

public class CreateUserException(string details) : CustomException(
    HttpStatusCode.BadRequest,
    $"Failed to register user. {details}.",
    "AUTH_REGISTRATION_ERROR");