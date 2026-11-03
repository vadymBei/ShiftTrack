using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Exceptions;

public class MessengerException(string errorMessage, string errorType, HttpStatusCode code = HttpStatusCode.BadRequest)
    : CustomException(code, errorMessage, $"CMN_MSG_{errorType}");
