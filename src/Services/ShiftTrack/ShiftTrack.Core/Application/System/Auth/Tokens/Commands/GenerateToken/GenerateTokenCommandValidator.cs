using FluentValidation;

namespace ShiftTrack.Core.Application.System.Auth.Tokens.Commands.GenerateToken
{
    public class GenerateTokenCommandValidator : AbstractValidator<GenerateTokenCommand>
    {
        public GenerateTokenCommandValidator()
        {
            RuleFor(x => x.Data.Login)
               .NotEmpty()
                   .WithMessage("Login is required.")
               .Matches(@"^\+\d{1,3}\s?\d{1,14}(\s?\d{1,13})?$")
                   .WithMessage("Login must be a valid international phone number.");


            RuleFor(v => v.Data.Password)
                .NotEmpty()
                    .WithMessage("Password is required.");
        }
    }
}
