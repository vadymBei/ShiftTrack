using FluentValidation;

namespace User.Authentication.Core.Application.Tokens.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(v => v.RefreshToken)
            .NotEmpty()
            .WithMessage("RefreshToken is required.");
    }
}