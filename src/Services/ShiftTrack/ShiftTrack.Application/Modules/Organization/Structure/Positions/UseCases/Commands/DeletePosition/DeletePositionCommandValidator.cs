using FluentValidation;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.DeletePosition;

public class DeletePositionCommandValidator : AbstractValidator<DeletePositionCommand>
{
    public DeletePositionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id is required");
    }
}