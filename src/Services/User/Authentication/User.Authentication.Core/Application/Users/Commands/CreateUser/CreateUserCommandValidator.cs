using FluentValidation;

namespace User.Authentication.Core.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required.")
            .Matches(@"^\+\d{1,3}\s?\d{1,14}(\s?\d{1,13})?$")
            .WithMessage("PhoneNumber must be a valid international phone number.");

        RuleFor(m => m.Email)
            .MaximumLength(64)
            .WithMessage("Maximum length must be 64 symbols")
            .EmailAddress()
            .WithMessage("Not valid email address");

        RuleFor(m => m.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MaximumLength(64)
            .WithMessage("Maximum length must be 64 symbols");
    }
}