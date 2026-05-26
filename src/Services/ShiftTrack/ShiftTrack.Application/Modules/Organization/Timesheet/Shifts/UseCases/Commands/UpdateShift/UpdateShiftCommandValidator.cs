using FluentValidation;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.UpdateShift;

public class UpdateShiftCommandValidator : AbstractValidator<UpdateShiftCommand>
{
    public UpdateShiftCommandValidator()
    {
        RuleFor(x => x.Data.Id)
            .NotNull()
            .WithMessage("Id is required")
            .Must(x => x > 0)
            .WithMessage("Id must be bigger than 0");

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