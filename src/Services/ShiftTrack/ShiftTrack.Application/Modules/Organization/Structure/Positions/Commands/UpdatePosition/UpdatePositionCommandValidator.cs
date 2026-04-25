using FluentValidation;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.Commands.UpdatePosition;

public class UpdatePositionCommandValidator : AbstractValidator<UpdatePositionCommand>
{
    public UpdatePositionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Maximum field length is 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(100)
            .WithMessage("Maximum field length is 100 characters");
        
        RuleFor(x => x.HourlyRate)
            .GreaterThan(0)
            .WithMessage("Hourly rate must be positive");
    }
}