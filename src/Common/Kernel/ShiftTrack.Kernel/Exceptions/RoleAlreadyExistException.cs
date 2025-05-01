using System.Net;

namespace ShiftTrack.Kernel.Exceptions;

public class RoleAlreadyExistException() : KernelException(
    HttpStatusCode.Forbidden,
    "Role already exist",
    "ROLE_ALREADY_EXIST");