using System.Net;

namespace ShiftTrack.Kernel.Exceptions;

public class RoleAlreadyExistException() : CustomException(
    HttpStatusCode.BadRequest,
    "Role already exist",
    $"SYS_USR_ROLE_ALREADY_EXIST");