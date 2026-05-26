using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Exceptions;

public class BusinessTripException(string errorMessage, string errorType)
    : CustomException(HttpStatusCode.BadRequest, errorMessage, $"BKG_BST_{errorType}");