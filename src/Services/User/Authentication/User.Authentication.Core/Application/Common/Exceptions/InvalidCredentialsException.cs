using ShiftTrack.Kernel.Exceptions;
using System.Net;

namespace User.Authentication.Core.Application.Common.Exceptions;

public class InvalidCredentialsException(string login, string details) : CustomException(HttpStatusCode.BadRequest,
    $"Invalid login or password for '{login}'. {details}",
    "AUTH_CREDENTIALS_ERROR");