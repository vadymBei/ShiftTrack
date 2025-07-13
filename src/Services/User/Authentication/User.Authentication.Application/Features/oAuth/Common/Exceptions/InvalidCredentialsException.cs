using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace User.Authentication.Application.Features.oAuth.Common.Exceptions;

public class InvalidCredentialsException(string login, string details) : CustomException(
    HttpStatusCode.BadRequest,
    $"Invalid login or password for '{login}'. {details}",
    "AUTH_CREDENTIALS_ERROR");