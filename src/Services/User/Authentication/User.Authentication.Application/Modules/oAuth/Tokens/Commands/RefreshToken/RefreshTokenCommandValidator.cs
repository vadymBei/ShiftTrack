using FluentValidation;

namespace User.Authentication.Application.Modules.oAuth.Tokens.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(v => v.RefreshToken)
            .NotEmpty()
            .WithMessage("RefreshToken is required.");
    }
}