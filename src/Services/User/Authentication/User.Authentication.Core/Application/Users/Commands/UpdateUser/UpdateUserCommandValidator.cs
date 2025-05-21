using FluentValidation;

namespace User.Authentication.Core.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");

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
    }
}