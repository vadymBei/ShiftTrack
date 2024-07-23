using ShiftTrack.Kernel.Constants;
using System.Net;

namespace ShiftTrack.Kernel.Exceptions
{
    public class AccessDeniedException : CustomException
    {
        public AccessDeniedException(List<string> allowedRoles) 
            : base(
                  HttpStatusCode.UnavailableForLegalReasons, 
                  $"Access allowed for roles: {string.Join(", ", allowedRoles)}", 
                  BaseErrorType.NotAllowedError)
        {

        }
        public AccessDeniedException(string message) 
            : base(
                  HttpStatusCode.UnavailableForLegalReasons, 
                  message,
                  BaseErrorType.NotAllowedError)
        {

        }
    }
}
