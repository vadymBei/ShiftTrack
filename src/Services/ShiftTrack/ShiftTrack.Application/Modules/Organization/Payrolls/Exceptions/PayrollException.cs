using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.Organization.Payrolls.Exceptions;

public class PayrollException(string errorMessage, string errorType)
    : CustomException(HttpStatusCode.BadRequest, errorMessage, $"ORG_PRL_{errorType}");