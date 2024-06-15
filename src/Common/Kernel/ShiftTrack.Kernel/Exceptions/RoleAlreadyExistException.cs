using ShiftTrack.Kernel.Constants;
using System.Net;

namespace ShiftTrack.Kernel.Exceptions
{
    public class RoleAlreadyExistException : KernelException
    {
        public RoleAlreadyExistException(string roleName) : base(
                  HttpStatusCode.Forbidden,
                  $"Role already exist with name: {roleName}",
                  BaseErrorType.EntityAlreadyExistError)
        {
        }
    }
}
