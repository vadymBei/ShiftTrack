using FluentValidation;

namespace User.Authentication.Core.Application.Tokens.Commands.GenerateToken
{
    public class GenerateTokenCommandValidator : AbstractValidator<GenerateTokenCommand>
    {
        public GenerateTokenCommandValidator()
        {
            RuleFor(x => x.PhoneNumber)
               .NotEmpty()
                   .WithMessage("PhoneNumber is required.")
               .Matches(@"^\+\d{1,3}\s?\d{1,14}(\s?\d{1,13})?$")
                   .WithMessage("PhoneNumber must be a valid international phone number.");


            RuleFor(v => v.Password)
                .NotEmpty()
                    .WithMessage("Password is required.");
        }
    }
}
