using FluentValidation;

namespace ShiftTrack.Application.Features.System.Auth.Account.Commands.UploadPhoto;

public class UploadPhotoCommandValidator : AbstractValidator<UploadPhotoCommand>
{
    public UploadPhotoCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotNull()
            .WithMessage("EmployeeId is required")
            .GreaterThan(0)
            .WithMessage("EmployeeId must be greater than zero.");
    }
}