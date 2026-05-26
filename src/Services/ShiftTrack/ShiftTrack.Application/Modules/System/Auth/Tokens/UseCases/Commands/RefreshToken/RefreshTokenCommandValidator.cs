using FluentValidation;

namespace ShiftTrack.Application.Modules.System.Auth.Tokens.UseCases.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.Data.RefreshToken)
            .NotEmpty()
            .WithMessage("RefreshToken is required.");
    }
}