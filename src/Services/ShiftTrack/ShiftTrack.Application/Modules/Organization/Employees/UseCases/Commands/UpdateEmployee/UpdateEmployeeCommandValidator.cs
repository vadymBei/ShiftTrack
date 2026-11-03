using FluentValidation;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Commands.UpdateEmployee;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id is required")
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .Length(1, 64)
            .WithMessage("Name must be between 1 and 64 characters.");

        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage("Surname is required.")
            .Length(1, 64)
            .WithMessage("Surname must be between 1 and 64 characters.");

        RuleFor(x => x.Patronymic)
            .Length(0, 64)
            .WithMessage("Patronymic can be up to 64 characters long.");
        
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.HireDate)
            .GreaterThanOrEqualTo(new DateTime(1950, 1, 1))
            .WithMessage("Hire date is too far in the past.")
            .LessThanOrEqualTo(_ => DateTime.Today.AddYears(1))
            .WithMessage("Hire date can be at most a year ahead.")
            .When(x => x.HireDate.HasValue);

        RuleFor(x => x.HireDate)
            .GreaterThan(x => x.DateOfBirth)
            .When(x => x.HireDate.HasValue && x.DateOfBirth.HasValue)
            .WithMessage("Hire date must be after the date of birth.");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .WithMessage("Invalid gender.");
    }
}