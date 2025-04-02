using FluentValidation;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.UpdateShift;

public class UpdateShiftCommandValidator : AbstractValidator<UpdateShiftCommand>
{
    public UpdateShiftCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id is required")
            .Must(x => x > 0)
            .WithMessage("Id must be bigger than 0");

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