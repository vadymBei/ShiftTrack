using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.System.User.Common.Exceptions;

public class EmployeeRoleAlreadyExistException(string errorMessage, string errorType) 
    : CustomException(HttpStatusCode.BadRequest, errorMessage, $"SYS_USR_{errorType}");