using FluentValidation;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift;

public class CreateShiftCommandValidator : AbstractValidator<CreateShiftCommand>
{
    public CreateShiftCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required");

        RuleFor(x => x.Color)
            .NotEmpty()
            .WithMessage("Color is required");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Color is required");

        RuleFor(x => x.Type)
            .NotNull()
            .WithMessage("Type is required");
    }
}