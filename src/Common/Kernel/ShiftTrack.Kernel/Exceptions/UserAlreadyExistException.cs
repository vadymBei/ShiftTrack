using System.Net;

namespace ShiftTrack.Kernel.Exceptions;

public class UserAlreadyExistException(string phoneNumber) : KernelException(
    HttpStatusCode.Forbidden,
    $"User already exist with phone number: {phoneNumber}",
    "USER_ALREADY_EXIST_WITH_PHONE_NUMBER");