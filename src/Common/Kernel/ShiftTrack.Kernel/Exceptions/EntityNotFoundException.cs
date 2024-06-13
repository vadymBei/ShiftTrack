using ShiftTrack.Kernel.Constants;
using System.Net;

namespace ShiftTrack.Kernel.Exceptions
{
    public class EntityNotFoundException : CustomException
    {
        public EntityNotFoundException()
            : base(HttpStatusCode.NotFound, "Entity not found exception", BaseErrorType.EntityNotFoundError)
        {

        }

        public EntityNotFoundException(string message)
            : base(HttpStatusCode.NotFound, message, BaseErrorType.EntityNotFoundError)
        {
        }

        public EntityNotFoundException(Type type, object entityId)
            : base(HttpStatusCode.NotFound, $"Entity '{type.Name}' was not found with Id: {entityId}", BaseErrorType.EntityNotFoundError)
        {
        }
    }
}
