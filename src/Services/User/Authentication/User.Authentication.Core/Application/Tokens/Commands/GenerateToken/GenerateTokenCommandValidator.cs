using FluentValidation;

namespace User.Authentication.Core.Application.Tokens.Commands.GenerateToken
{
    public class GenerateTokenCommandValidator : AbstractValidator<GenerateTokenCommand>
    {
        public GenerateTokenCommandValidator()
        {
            RuleFor(v => v.Login)
                .NotEmpty()
                    .WithMessage("Login is required.");

            RuleFor(v => v.Password)
                .NotEmpty()
                    .WithMessage("Password is required.");
        }
    }
}
