using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Booking.Common.Exceptions;

public class VacationException(string errorMessage, string errorType)
    : CustomException(HttpStatusCode.BadRequest, errorMessage, $"BCK_VCN_{errorType}");