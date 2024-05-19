using FluentValidation.Results;
using Kernel.Constants;
using System.Net;

namespace Kernel.Exceptions
{
    public class ValidationException : KernelException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base(
                  HttpStatusCode.UnprocessableEntity,
                  "One or more validation failures have occurred.",
                  BaseErrorType.ValidationError)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}
