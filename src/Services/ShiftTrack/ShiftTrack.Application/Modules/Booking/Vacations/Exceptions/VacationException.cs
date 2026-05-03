using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;

public class VacationException(string errorMessage, string errorType)
    : CustomException(HttpStatusCode.BadRequest, errorMessage, $"BCK_VCN_{errorType}");