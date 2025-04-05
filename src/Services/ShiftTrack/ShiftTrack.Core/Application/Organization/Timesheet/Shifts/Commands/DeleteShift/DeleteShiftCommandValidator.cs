using FluentValidation;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.DeleteShift;

public class DeleteShiftCommandValidator : AbstractValidator<DeleteShiftCommand>
{
    public DeleteShiftCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id is required")
            .Must(x => x > 0)
            .WithMessage("Id must be bigger than 0");
    }
}