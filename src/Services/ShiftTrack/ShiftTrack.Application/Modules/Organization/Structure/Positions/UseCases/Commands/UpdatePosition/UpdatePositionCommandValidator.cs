using FluentValidation;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.UpdatePosition;

public class UpdatePositionCommandValidator : AbstractValidator<UpdatePositionCommand>
{
    public UpdatePositionCommandValidator()
    {
        RuleFor(x => x.Data.Id)
            .NotNull()
            .WithMessage("Id is required");

        RuleFor(x => x.Data.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Maximum field length is 100 characters");

        RuleFor(x => x.Data.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(100)
            .WithMessage("Maximum field length is 100 characters");
        
        RuleFor(x => x.Data.HourlyRate)
            .GreaterThan(0)
            .WithMessage("Hourly rate must be positive");
    }
}