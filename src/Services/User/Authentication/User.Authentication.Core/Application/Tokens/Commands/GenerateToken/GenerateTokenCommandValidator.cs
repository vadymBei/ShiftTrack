using FluentValidation;

namespace User.Authentication.Core.Application.Tokens.Commands.GenerateToken
{
    public class GenerateTokenCommandValidator : AbstractValidator<GenerateTokenCommand>
    {
        public GenerateTokenCommandValidator()
        {
            RuleFor(x => x.Login)
               .NotEmpty()
                   .WithMessage("Login is required.")
               .Matches(@"^\+\d{1,3}\s?\d{1,14}(\s?\d{1,13})?$")
                   .WithMessage("Login must be a valid international phone number.");


            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Password is required.");
        }
    }
}
