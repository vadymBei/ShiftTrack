using FluentValidation;

namespace User.Authentication.Core.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(m => m.Email)
                .NotEmpty()
                    .WithMessage("Email is required")
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
}
