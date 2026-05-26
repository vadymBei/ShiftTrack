using FluentValidation;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.CreateShift;

public class CreateShiftCommandValidator : AbstractValidator<CreateShiftCommand>
{
    public CreateShiftCommandValidator()
    {
        RuleFor(x => x.Data.Code)
            .NotEmpty()
            .WithMessage("Code is required");

        RuleFor(x => x.Data.Color)
            .NotEmpty()
            .WithMessage("Color is required");

        RuleFor(x => x.Data.Description)
            .NotEmpty()
            .WithMessage("Color is required");

        RuleFor(x => x.Data.Type)
            .NotNull()
            .WithMessage("Type is required");

        When(x => x.Data.StartTime.HasValue && x.Data.EndTime.HasValue, () =>
        {
            RuleFor(x => x.Data.EndTime)
                .GreaterThan(x => x.Data.StartTime.Value)
                .WithMessage("EndTime must be greater than StartTime");
        });
    }
}