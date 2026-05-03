using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Exceptions;

public class EmployeeRoleAlreadyExistException(string errorMessage, string errorType)
    : CustomException(HttpStatusCode.BadRequest, errorMessage, $"SYS_USR_{errorType}");