using ShiftTrack.Kernel.Exceptions;
using System.Net;

namespace User.Authentication.Core.Application.Common.Exceptions;

public class CreateRoleException(string details) : CustomException(
    HttpStatusCode.BadRequest,
    $"Failed to create role. {details}.",
    "AUTH_CREATE_ROLE_ERROR");