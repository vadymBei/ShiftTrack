using FluentValidation;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.UploadPhoto;

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