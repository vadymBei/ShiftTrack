using FluentValidation;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty()
                   .WithMessage("Id is required")
               .GreaterThan(0)
                   .WithMessage("Id must be bigger than 0");

            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Name is required")
                .MaximumLength(100)
                    .WithMessage("Maximum field length is 100 characters");
        }
    }
}
