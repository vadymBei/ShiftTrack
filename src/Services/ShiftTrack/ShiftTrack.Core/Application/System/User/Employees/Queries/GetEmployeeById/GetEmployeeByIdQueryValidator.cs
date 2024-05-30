using FluentValidation;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryValidator : AbstractValidator<GetEmployeeByIdQuery>
    {
        public GetEmployeeByIdQueryValidator()
        {
            RuleFor(x => x.Id)
              .NotNull()
                  .WithMessage("Id is required");
        }
    }
}
