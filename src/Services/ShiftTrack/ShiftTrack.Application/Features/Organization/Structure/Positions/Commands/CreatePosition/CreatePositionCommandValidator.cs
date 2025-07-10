using FluentValidation;

namespace ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.CreatePosition;

public class CreatePositionCommandValidator : AbstractValidator<CreatePositionCommand>
{
    public CreatePositionCommandValidator()
    {
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
    }
}