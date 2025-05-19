using FluentValidation;

namespace User.Authentication.Core.Application.Users.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Data.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(BeValidGuid)
            .WithMessage("UserId must be a valid GUID.");

        RuleFor(x => x.Data.OldPassword)
            .NotEmpty()
            .WithMessage("Old password is required.")
            .MaximumLength(64)
            .WithMessage("Old password maximum length must be 64 symbols");

        RuleFor(x => x.Data.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .MaximumLength(64)
            .WithMessage("New password maximum length must be 64 symbols");

        RuleFor(x => x.Data.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm password is required.")
            .Equal(x => x.Data.NewPassword)
            .WithMessage("Confirm password must match the new password.");
    }

    private bool BeValidGuid(string guid)
    {
        return Guid.TryParse(guid, out _);
    }
}