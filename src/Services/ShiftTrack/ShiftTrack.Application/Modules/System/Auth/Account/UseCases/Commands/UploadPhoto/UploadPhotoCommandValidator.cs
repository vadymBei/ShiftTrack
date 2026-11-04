using FluentValidation;
using ShiftTrack.Application.Modules.Organization.Employees.Constants;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.UploadPhoto;

public class UploadPhotoCommandValidator : AbstractValidator<UploadPhotoCommand>
{
    public UploadPhotoCommandValidator()
    {
        RuleFor(x => x.File)
            .Must(x => x is { Length: > 0 })
            .WithMessage(EmployeeExceptionsLocalization.PHOTO_REQUIRED);
    }
}
