using FluentValidation;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Commands.CreateShift;

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

        When(x => x.StartTime.HasValue && x.EndTime.HasValue, () =>
        {
            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime.Value)
                .WithMessage("EndTime must be greater than StartTime");
        });
    }
}