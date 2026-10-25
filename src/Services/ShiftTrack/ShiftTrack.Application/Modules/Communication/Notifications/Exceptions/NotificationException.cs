using System.Net;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.Communication.Notifications.Exceptions;

public class NotificationException(string errorMessage, string errorType)
    : CustomException(HttpStatusCode.BadRequest, errorMessage, $"CMN_NTF_{errorType}");
