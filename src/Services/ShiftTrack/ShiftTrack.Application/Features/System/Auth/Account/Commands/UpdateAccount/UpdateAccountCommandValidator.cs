using FluentValidation;

namespace ShiftTrack.Application.Features.System.Auth.Account.Commands.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
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

        RuleFor(v => v.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .Length(5, 64)
            .WithMessage("Email lenght min 5, max 64")
            .EmailAddress()
            .WithMessage("Email is not valid");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required.")
            .Matches(@"^\+\d{1,3}\s?\d{1,14}(\s?\d{1,13})?$")
            .WithMessage("PhoneNumber must be a valid international phone number.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .WithMessage("Invalid gender.");
    }
}