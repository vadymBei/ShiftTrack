using FluentValidation;

namespace ShiftTrack.Core.Application.System.Auth.Tokens.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.Data.RefreshToken)
                .NotEmpty()
                    .WithMessage("RefreshToken is required.");
        }
    }
}
