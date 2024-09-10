using FluentValidation;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Name is required")
                .MaximumLength(100)
                    .WithMessage("Maximum field length is 100 characters");
        }
    }
}
