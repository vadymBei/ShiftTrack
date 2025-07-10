using FluentValidation;

namespace ShiftTrack.Application.Features.Organization.Structure.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required")
            .NotNull()
            .WithMessage("Id is required")
            .GreaterThan(0)
            .WithMessage("Id must be bigger than 0");
    }
}