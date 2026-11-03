using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.Organization.Employees.Exceptions;

public class EmployeeException(string errorMessage, string errorType, HttpStatusCode code = HttpStatusCode.BadRequest)
    : CustomException(code, errorMessage, $"ORG_EMP_{errorType}");
