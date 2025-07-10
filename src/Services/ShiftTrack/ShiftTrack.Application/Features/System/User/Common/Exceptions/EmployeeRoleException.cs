using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.System.User.Common.Exceptions;

public class EmployeeRoleException(string errorMessage, string errorType)
    : CustomException(HttpStatusCode.BadRequest, errorMessage, $"SYS_USR_{errorType}");