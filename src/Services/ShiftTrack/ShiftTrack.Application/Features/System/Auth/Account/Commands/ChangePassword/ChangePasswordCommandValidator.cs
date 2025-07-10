using FluentValidation;

namespace ShiftTrack.Application.Features.System.Auth.Account.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Data.EmployeeId)
            .NotNull()
            .WithMessage("EmployeeId is required")
            .GreaterThan(0)
            .WithMessage("EmployeeId must be greater than zero.");

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
}